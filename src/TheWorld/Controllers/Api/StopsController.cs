using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheWorld.Entities;
using TheWorld.Services;
using TheWorld.ViewModels;

namespace TheWorld.Controllers.Api
{
    [Authorize]
    [Route("/api/trips/{tripName}/stops")]
    public class StopsController : Controller
    {
        #region Initialize
        private GeoCoordsService geoCoordService;
        private ILogger<StopsController> logger;
        private IWorldRepository repository;

        public StopsController(IWorldRepository repository, ILogger<StopsController> logger, GeoCoordsService geoCoordService)
        {
            this.repository = repository;
            this.logger = logger;
            this.geoCoordService = geoCoordService;
        }
        #endregion


        #region Methods
        [HttpGet("")]
        public IActionResult Get(string tripName)
        {
            try
            {
                var trip = this.repository.GetTripsByUserName(tripName, User.Identity.Name);
                return Ok(Mapper.Map<IEnumerable<StopViewModel>>(trip.Stops.OrderBy(s => s.Order).ToList()));
            }
            catch (Exception ex)
            {
                logger.LogError("Unable to get Stops: {0}", ex);
            }
            return BadRequest("Unable to get Stops");
        }

        [HttpPost("")]
        public async Task<IActionResult> Post(string tripName, [FromBody] StopViewModel stopViewModel)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    var newStop = Mapper.Map<Stop>(stopViewModel);
                    var result = await this.geoCoordService.GetGeoCoordsAsync(newStop.Name);
                    if (result.Success)
                    {
                        newStop.Latitude = result.Latitude;
                        newStop.Longitude = result.Longitude;

                        this.repository.AddStop(tripName, newStop, User.Identity.Name);
                        if (await this.repository.SaveChangesAsync())
                        {
                            return Created($"/api/trips/{tripName}/stops/{newStop.Name}", Mapper.Map<StopViewModel>(newStop));
                        }
                    }
                    else
                    {
                        this.logger.LogError(result.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                this.logger.LogError("Unable to save Stop. An exception was thrown: {0}", ex);
            }
            return BadRequest("Unable to save the Stop.");
        }
        #endregion
    }
}
