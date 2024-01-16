using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using CV.Models;
using Microsoft.AspNetCore.Authorization;
using Models;

namespace CV.Controllers
{
	public class AccountController : Controller
	{
		private UserManager<LogInUser> userManager;
		private SignInManager<LogInUser> signInManager;
		private readonly UserContext _userContext;

		public AccountController(UserManager<LogInUser> userMngr, SignInManager<LogInUser> signInMngr, UserContext userContext)
		{
			userManager = userMngr;
			signInManager = signInMngr;
			_userContext = userContext;
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

				var existingUser = await userManager.FindByNameAsync(arm.UserName);
				if (existingUser != null)
				{
					ModelState.AddModelError("UserName", "Användarnamnet är redan taget.");
					return View(arm);
				}

           

                LogInUser user = new LogInUser();
				user.UserName = arm.UserName;
				user.Email = arm.Epost;


				var result = await userManager.CreateAsync(user, arm.Password);

				if (result.Succeeded)
				{
					User customUser = new User
					{
						Username = arm.UserName,
						Password = arm.Password,
						ConfirmPassword = arm.ConfirmPassword,
						Firstname = arm.FirstName,
						Lastname = arm.LastName,
						Epost = arm.Epost,
						Privat = arm.Privat,
						
					};

				
					_userContext.Users.Add(customUser);
					_userContext.SaveChanges();


                    CV_ pic = new CV_();
					pic.UID = customUser.UID;
					pic.Picture = "no_profile_picture.jpg";



                    _userContext.CV_s.Add(pic);
                    _userContext.SaveChanges();

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

