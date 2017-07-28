using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheWorld.Entities;
using TheWorld.ViewModels;

namespace TheWorld.Controllers.Web
{
    public class AuthController : Controller
    {
        #region Initialize
        private SignInManager<WorldUser> signInManager;

        public AuthController(SignInManager<WorldUser> signInManager)
        {
            this.signInManager = signInManager;
        }
        #endregion

        #region Methods
        public ActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Trips", "App");
            }
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
                var signInResult = await this.signInManager.PasswordSignInAsync(loginViewModel.UserName, 
                    loginViewModel.Password, false, false);

                if (signInResult.Succeeded)
                {
                    return RedirectToAction("Trips", "App");
                }
            }

            ModelState.AddModelError("", "Unable to Login");
            return View();
        }

        public async Task<ActionResult> Logout()
        {
            if (User.Identity.IsAuthenticated)
            {
                await this.signInManager.SignOutAsync();
            }

            return RedirectToAction("Index", "App");
        }
        #endregion
    }
}
