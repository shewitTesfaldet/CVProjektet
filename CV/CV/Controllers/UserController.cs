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
                return View(newUser); }


        }
        [HttpGet]
        [Authorize]
        public IActionResult Profile()
        {
            string loginUser = User.Identity.Name;

            // Gör jämförelsen icke-casesensitiv direkt i databasfrågan
            User user = _userContext.Users.FirstOrDefault(u => u.Username.ToUpper() == loginUser.ToUpper());

            if (user == null)
            {
                return NotFound();
            }

            ViewBag.Profile = user;
            return View(user);
        }


        [HttpPost]
        public IActionResult Profile(User updatedUser, string loginUser)
        {
            if (ModelState.IsValid)
            {
                User existingUser = _userContext.Users.FirstOrDefault(x=> x.Equals(loginUser));

                if (existingUser == null)
                {
                    return NotFound();
                }

                existingUser.Password = updatedUser.Password;
                existingUser.ConfirmPassword = updatedUser.ConfirmPassword;
                existingUser.Firstname = updatedUser.Firstname;
                existingUser.Lastname = updatedUser.Lastname;
                existingUser.Epost = updatedUser.Epost;
                existingUser.Adress = updatedUser.Adress;
                existingUser.Privat = updatedUser.Privat;
                existingUser.Username = updatedUser.Username;
                // Spara ändringarna i databasen
                _userContext.SaveChanges();

                // Uppdatera ViewBag.Message med ett meddelande som bekräftar ändringarna
                ViewBag.Message = "Profilen har uppdaterats";

                // Återvänd till profilsidan
                return View(existingUser);
            }
            else
            {
                // Om modellens tillstånd inte är giltigt, returnera vyn med fel
                return View(updatedUser);
            }
        }
    }
}
