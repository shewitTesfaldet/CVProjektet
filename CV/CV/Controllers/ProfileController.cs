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

            var pictureList = (from id in _userContext.CV_s
                               select id.CID).ToList();


            for (int pid = 0; pid < pictureList.Count(); pid++)
            {

                int expIdList = pictureList.ElementAt(pid);

                if (LogedInID == expIdList)
                {

                    var profilePicture = (from cv in _userContext.CV_s
                                          where cv.UID == LogedInID
                                          select cv.Picture).FirstOrDefault();

                    ViewBag.ProfilePicture = profilePicture;

                }



            }

            if (!pictureList.Contains(LogedInID))
            {
                string filePath = "C:\\Users\\Admin\\OneDrive\\Dokument\\Webbsystem(.NET)\\CVProjekt\\CV\\CV\\wwwroot\\Pictures\\" + ViewBag.noProfilePicture + "";


                string FilePathExists = Path.Combine(filePath);

                if (!System.IO.File.Exists(FilePathExists))
                {
                    ViewBag.noProfilePicture = "no_profile_picture.jpg";
                }
            }

            return View(user);
        }



        [Authorize]
        [HttpGet]
        public IActionResult UpdateProfile()
        {
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

            
            if (ModelState.IsValid)
            {
				/*				string fullpath = await GetPicture(CVBild);
				 *				
				*/

				string fileName = CVBild.FileName;
                string path = "C:\\Users\\Admin\\OneDrive\\Dokument\\Webbsystem(.NET)\\CVProjekt\\CV\\CV\\wwwroot\\Pictures\\";


				// Kontrollera om filen är null
				if (CVBild == null || CVBild.Length == 0)
				{
					string filePath = path + ViewBag.noProfilePicture + "";


					string FilePathExists = Path.Combine(filePath);

					if (!System.IO.File.Exists(FilePathExists))
					{
						ViewBag.noProfilePicture = "no_profile_picture.jpg";
					}
				}

				string fullPath = Path.Combine(path, fileName);

				// Skriv filen till sökvägen
				using (var stream = new FileStream(fullPath, FileMode.Create))
				{
					await CVBild.CopyToAsync(stream);
				}
				CV_ LogedInCV = _userContext.CV_s.FirstOrDefault(x => x.UID.Equals(LogedInID));
                if (LogedInCV == null)
                {
                    CV_ NewCV = new CV_();
                    NewCV.UID = LogedInID;
                    NewCV.Picture = ViewBag.noProfilePicture;
                    _userContext.CV_s.Add(NewCV);

                }
                else
                {
					LogedInCV.Picture = fileName;
                    ViewBag.ProfilePicture = fileName;

				}

				_userContext.SaveChanges();


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

            return View("Profile", "Profile");
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
