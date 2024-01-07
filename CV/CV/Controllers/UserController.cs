using Models;
using CV.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;

namespace CV.Controllers
{
    public class UserController : Controller
    {
        private UserContext _userContext;

        public UserController(UserContext userContext)
        {
            _userContext = userContext;
        }


        public IActionResult Add()
        {
            return View(new User());
        }

        [HttpPost]

        public IActionResult Add(User newUser)
        {
            if (ModelState.IsValid)
            {

                _userContext.Users.Add(newUser);
                _userContext.SaveChanges();
                ViewBag.Message = $"Registration successful! Welcome, {newUser.Firstname} {newUser.Lastname}.";

                return RedirectToAction("Add", "User");
            }

            else
            {
                return View(newUser);
            }


        }

    }
}


