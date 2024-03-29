﻿using CV.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace CV.Controllers
{
    public class ProfileController : Controller
    {
        private UserContext _userContext;
        private MessageService _messageService;    

     
        public ProfileController(UserContext userContext, MessageService messageService)
        {
            _userContext = userContext;
            _messageService = messageService;
        }


    

    [Authorize]
        [HttpGet]
        public IActionResult Profile()
        {

            var currentUsername = User.Identity.Name;
            var hasUnreadMessages = _messageService.HasUnreadMessages(currentUsername);
            ViewBag.HasUnreadMessages = hasUnreadMessages;
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

            /*            ProfilBild
        */
            int LogedInID = _userContext.Users
                              .Where(x => x.Username.Equals(Name))
                              .Select(x => x.UID)
                              .FirstOrDefault();

            

                    var profilePicture = (from cv in _userContext.CV_s
                                          where cv.UID == LogedInID
                                          select cv.Picture).FirstOrDefault();

                    ViewBag.ProfilePicture = profilePicture;




            return View(user);
        }



        [Authorize]
        [HttpGet]
        public IActionResult UpdateProfile()
        {
            var currentUsername = User.Identity.Name;
            var hasUnreadMessages = _messageService.HasUnreadMessages(currentUsername);
            ViewBag.HasUnreadMessages = hasUnreadMessages;

            User user = new User();
            string Name = User.Identity.Name;

            //Hämtar ID på den inloggade
           int LogedInID = _userContext.Users
                               .Where(x => x.Username.Equals(Name))
                               .Select(x => x.UID)
                               .FirstOrDefault();

            //Hämtar bild på den inloggade
            var profilePicture = (from cv in _userContext.CV_s
                                  where cv.UID == LogedInID
                                  select cv.Picture).FirstOrDefault();

            ViewBag.ProfilePicture = profilePicture;



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

        public async Task<IActionResult> UpdateProfile(IFormFile CVBild, User updatedUser)
        {
            string Name = User.Identity.Name;
            int LogedInID = _userContext.Users
                              .Where(x => x.Username.Equals(Name))
                              .Select(x => x.UID)
                              .FirstOrDefault();
            string fileName = (from cv in _userContext.CV_s
                                                    where cv.UID == LogedInID
                                                    select cv.Picture).FirstOrDefault(); 

            string path = Directory.GetCurrentDirectory() + "\\wwwroot\\Pictures\\";

            //om du har skickat in en bild hämtas filnamnet
            if (CVBild != null)
            {
                fileName = CVBild.FileName;

            }
            // Om du inte skickat in något så ge

            string fullPath = Path.Combine(path, fileName);

            if (CVBild != null)
            {
                // Skriv filen till sökvägen endast om du skickat in en bild
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await CVBild.CopyToAsync(stream);
                }

                CV_ LogedInCV = _userContext.CV_s.FirstOrDefault(x => x.UID.Equals(LogedInID));
                LogedInCV.Picture = fileName;

            }
           
                ViewBag.ProfilePicture = fullPath;
            

            _userContext.SaveChanges();


            ModelState.Remove("CVBild");

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

               
                    //Ändrar bild



                    if (!string.IsNullOrEmpty(updatedUser.ConfirmPassword) && updatedUser.Password == updatedUser.ConfirmPassword)
                    {
                        updatedUser.Password = updatedUser.ConfirmPassword;
                    }

                    _userContext.SaveChanges();

                    // Ladda om sidan med de nya värdena
                    return View("Profile", userToUpdate);
                }
            }
            if (!ModelState.IsValid)
            {
                return View(updatedUser);
            }

            return View("Profile", "Profile");
        }


        [HttpGet]
        public IActionResult ViewProfile(string username)
        {
            var currentUsername = User.Identity.Name;
            var hasUnreadMessages = _messageService.HasUnreadMessages(currentUsername);
            ViewBag.HasUnreadMessages = hasUnreadMessages;
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
