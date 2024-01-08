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
        public IActionResult UpdateProfile(User updatedUser, string newEducation, string newCompetence, string newExperience)
        {
            string Name = User.Identity.Name;
            int? LoggedInID = _userContext.Users
                .Where(x => x.Username.Equals(Name))
                .Select(x => x.UID)
                .FirstOrDefault();

            int result = _userContext.CV_s
                .Where(cv => cv.UID == LoggedInID)
                .Select(cv => cv.CID)
                .FirstOrDefault();

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

                    Education education = new Education();
                    education.Description = newEducation;
                    education.EdID = result;

                    Competence competence = new Competence();
                    competence.Description = newCompetence;
                    competence.CompID = result;

                    Experience experience = new Experience();
                    experience.Description = newExperience;
                    experience.EID = result;

                    _userContext.Education.Add(education);
                    _userContext.Competence.Add(competence);
                    _userContext.Experience.Add(experience);

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
        
       

    }
}
