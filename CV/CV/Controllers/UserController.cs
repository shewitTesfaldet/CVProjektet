using Models;
using CV.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

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

        public IActionResult Profile(int UID)
        {
            UID = 3;
            User user = _userContext.Users.Find(UID);
            ViewBag.Profile = user;
            return View(user);
        }

        [HttpPost]
        public IActionResult Profile(User updatedUser)
        {
            if (ModelState.IsValid)
            {
                // Hitta användaren med det specifika UID (t.ex. 3) i databasen
                var existingUser = _userContext.Users.Find(3);

                if (existingUser == null)
                {
                    // Om användaren inte finns, hantera detta scenario, t.ex. visa felmeddelande.
                    return NotFound();
                }

                // Uppdatera befintliga användarvärden med de nya värdena från updatedUser
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
