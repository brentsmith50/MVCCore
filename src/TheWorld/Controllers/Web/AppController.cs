using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TheWorld.Services;
using Microsoft.Extensions.Configuration;
using TheWorld.ViewModels;
using TheWorld.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;

namespace TheWorld.Controllers.Web
{
    public class AppController : Controller
    {
        #region Fields
        private IConfigurationRoot configuration;
        private IMailService mailService;
        private IWorldRepository repository;
        private ILogger<AppController> logger;
        #endregion

        #region Constructors
        public AppController(IMailService mailService, IConfigurationRoot configuration, IWorldRepository repository, ILogger<AppController> logger)
        {
            this.mailService = mailService;
            this.configuration = configuration;
            this.repository = repository;
            this.logger = logger;
        }
        #endregion

        #region Action Methods
        public IActionResult Index()
        {
            return View();
        }

        //[Authorize]
        public IActionResult Trips()
        {
            return View();
            //try
            //{
            //    var data = this.repository.GetAllTrips();
            //    return View(data);
            //}
            //catch (Exception ex)
            //{
            //    logger.LogError($"An Error occured getting all Trips: {ex.Message}");
            //    return Redirect("/error");
            //}
        }

        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Contact(ContactViewModel contactViewModel)
        {
            if (contactViewModel.Email.Contains("aol.com"))
            {
                ModelState.AddModelError("", "We don't support AOL email addresses.");
            }

            if (ModelState.IsValid)
            {
                mailService.SendMail(configuration["MailSettings:ToAddress"], contactViewModel.Email, "From: The World", contactViewModel.Message);
                ModelState.Clear();
                ViewBag.UserMessage = "Message Sent";
            }

            return View();
        }

        public IActionResult About()
        {
            return View();
        }
        #endregion

    }
}
