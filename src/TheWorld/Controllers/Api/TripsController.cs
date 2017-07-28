using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheWorld.Entities;
using TheWorld.ViewModels;

namespace TheWorld.Controllers.Api
{
    [Authorize]
    [Route("api/trips")]
    public class TripsController : Controller
    {
        private ILogger<TripsController> logger;
        private IWorldRepository repository;

        public TripsController(IWorldRepository repository, ILogger<TripsController> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        #region Methods
        [HttpGet("")]
        public IActionResult Get()
        {
            try
            {
                var results = this.repository.GetTripsByUserName(User.Identity.Name);
                return Ok(Mapper.Map<IEnumerable<TripViewModel>>(results));
            }
            catch (Exception ex)
            {
                this.logger.LogError($"An Exception occured getting trips: {ex}");
                return BadRequest("An Error Occured.");
            }

        }

        [HttpPost("")]
        public async Task<IActionResult> Post([FromBody] TripViewModel theTrip)
        {
            if (ModelState.IsValid)
            {
                var newTrip = Mapper.Map<Trip>(theTrip);
                newTrip.UserName = User.Identity.Name;
                repository.AddTrip(newTrip);
                if (await this.repository.SaveChangesAsync())
                {
                    return Created($"api/trips/{theTrip.Name}", Mapper.Map<TripViewModel>(newTrip));
                }
            }
            return BadRequest("An error occured while saving the trip");
        }
        #endregion
    }
}
