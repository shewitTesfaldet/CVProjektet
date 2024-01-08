using Models;
using CV.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;

namespace CV.Controllers
{
    public class ProfileController : Controller
    {
        private UserContext _userContext;

        public ProfileController(UserContext userContext)
        {
            _userContext = userContext;
        }

        [Authorize]
        [HttpGet]
        public IActionResult Profile()
        {
            User user = new User();
            string Name = User.Identity.Name;

            if (Name != null)
            {
                user = _userContext.Users.FirstOrDefault(x => x.Username.Equals(Name));
            }
            if (user.Username == null)
            {
                return NotFound();
            }
            ViewBag.UID = user.UID;

            return View(user);
        }



        [Authorize]
        [HttpGet]
        public IActionResult UpdateProfile()
        {
            User user = new User();
            string Name = User.Identity.Name;

            if (Name != null)
            {
                user = _userContext.Users.FirstOrDefault(x => x.Username.Equals(Name));
            }
            if (user.Username == null)
            {

                return NotFound();
            }


            return View(user);
        }


        [HttpPost]
        public IActionResult UpdateProfile(User updatedUser)
        {
            string Name = User.Identity.Name;

            if (ModelState.IsValid)
            {
                var userToUpdate = _userContext.Users.FirstOrDefault(x => x.Username.Equals(Name));

                if (userToUpdate != null)
                {
                    userToUpdate.Firstname = updatedUser.Firstname;
                    userToUpdate.Lastname = updatedUser.Lastname;
                    userToUpdate.Password = updatedUser.Password;
                    userToUpdate.Adress = updatedUser.Adress;
                    userToUpdate.Epost = updatedUser.Epost;
                    userToUpdate.Privat = updatedUser.Privat;


                    if (!string.IsNullOrEmpty(updatedUser.ConfirmPassword) && updatedUser.Password == updatedUser.ConfirmPassword)
                    {
                        updatedUser.Password = updatedUser.ConfirmPassword;
                    }

                    _userContext.SaveChanges();

                    // Ladda om sidan med de nya värdena
                    return View("Profile", userToUpdate);
                }
            }

            return View("Profile", updatedUser);
        }

        [HttpGet]
        public IActionResult ViewProfile(string username)
        {
            var user = _userContext.Users.FirstOrDefault(x => x.Username.Equals(username));

            if (user == null)
            {
                return NotFound();
            }

            // Kontrollera om användaren har valt att vara privat
            if (user.Privat)
            {
                // Användaren är privat, skicka ett felmeddelande till vyn
                ViewData["ErrorMessage"] = "Den här profilen är privat.";
            }

            // Användaren är inte privat, visa profilen
            return View(user);
        }
        [HttpPost]
        public IActionResult UpdateProfile(User updatedUser, IFormFile profilePicture)
        {
            string Name = User.Identity.Name;

            if (ModelState.IsValid)
            {
                // Other updates here...

                // Get the current user including the CV
                var currentUser = _userContext.Users.Include(u => u.CV_).FirstOrDefault(x => x.Username.Equals(Name));

                // Update user properties
                currentUser.Firstname = updatedUser.Firstname;
                currentUser.Lastname = updatedUser.Lastname;
                // ... (other properties)

                // Save the uploaded picture if it exists
                if (profilePicture != null && profilePicture.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        profilePicture.CopyTo(memoryStream);
                        if (currentUser.CV_ == null)
                        {
                            currentUser.CV_ = new CV_(); // Assuming CV_ is your CV model
                        }
                        currentUser.CV_.Picture = Convert.ToBase64String(memoryStream.ToArray());
                    }
                }

                _userContext.SaveChanges();

                return RedirectToAction("Profile");
            }

            return View(updatedUser);
        }

    }
}
