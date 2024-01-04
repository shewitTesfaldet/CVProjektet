using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using CV.Models;
using Microsoft.AspNetCore.Authorization;

namespace CV.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<LogInUser> userManager;
        private SignInManager<LogInUser> signInManager;

        public AccountController(UserManager<LogInUser> userMngr, SignInManager<LogInUser> signInMngr) 
        {
            userManager = userMngr;
            signInManager = signInMngr;
        }

        [HttpGet]        
        public IActionResult LogIn(string returnURL = "")
        {
            LoginViewModel loginViewModel = new LoginViewModel();
            return View(loginViewModel);
        }

        [HttpPost]
		public async Task<IActionResult> LogIn(LoginViewModel loginViewModel)
		{
			if (ModelState.IsValid)
			{
				var result = await signInManager.PasswordSignInAsync(loginViewModel.UserName, loginViewModel.Password, isPersistent: loginViewModel.RememberMe, lockoutOnFailure: false);

				if (result.Succeeded)
				{
					return RedirectToAction("Index", "Home");
				}
				else
				{
					ModelState.AddModelError(string.Empty, "Ogiltigt användarnamn eller lösenord.");
				}
			}
			return View(loginViewModel);
		}


		[HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]

        public async Task<IActionResult> Register(UserRegisterViewModel arm)
        {
            if (ModelState.IsValid)
            {
                LogInUser user = new LogInUser();
                user.UserName = arm.UserName;

                var result = await userManager.CreateAsync(user, arm.Password);
                if (result.Succeeded)
                {
                    await signInManager.SignInAsync(user, isPersistent: true);
                    return RedirectToAction("Index", "Home");

                }
              
            }
            return View(arm);
        }




        [HttpPost]
        public async Task<IActionResult> LogOut()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
    
    }
    }

}
