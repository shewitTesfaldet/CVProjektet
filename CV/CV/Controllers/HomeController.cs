using CV.Models;
using Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Security.Cryptography;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using System;

namespace CV.Controllers
{
    public class HomeController : Controller
    {
        private UserContext _userContext;
        public HomeController(UserContext usercontext)
        {
            _userContext = usercontext;
        }


      
        public  IActionResult Index()
        {
            //H�mtar alla anv�ndare med utbildning i en lista

            var data =  (from user in  _userContext.Users
                        join cv in _userContext.CV_s on user.UID equals cv.UID into cvGroup
                        from cv in cvGroup.DefaultIfEmpty()
                        join cvEdu in _userContext.CV_Educations on cv.CID equals cvEdu.CID into cvEduGroup
                        from cvEdu in cvEduGroup.DefaultIfEmpty()
                        join edu in _userContext.Education on cvEdu.EdID equals edu.EdID into eduGroup
                        from edu in eduGroup.DefaultIfEmpty()
                        select new { user.UID, user.Firstname, user.Lastname, user.Username, user.Privat, Description = (edu == null ? null : edu.Description) }).ToList();

            //Sorterar bort alla anonyma anv�ndare
			data = data.Where(user => user.Firstname != "Anonym").ToList();

			//sorterar listan utofr�n de senaste inlaggda och de 5 f�rsta.


			var result = data.GroupBy(x => x.UID)
                             .Select(g => g.First())
                             .OrderByDescending(x => x.UID)
                             .Take(5)
                             .ToList();

            ViewBag.Result = result;
            //H�mtar projekt utifr�n anv�ndare och sorterar de senaste
            var Presult =  (from user in _userContext.Users
                           join userProject in _userContext.UserProjects on user.UID equals userProject.UID
                           join project in _userContext.Projects on userProject.PID equals project.PID
                           orderby project.PID descending
                           select new { user.Username, project.PID, project.Title, project.Description })
             .Take(5)
             .ToList();

            ViewBag.Presult = Presult;


            return View();

        }


        [Authorize]
        public IActionResult JoinProject(int PID)
        {
            // Get the current user's UID
            var currentUsername = User.Identity.Name;
            var currentUser = _userContext.Users.SingleOrDefault(u => u.Username == currentUsername);

            if (currentUser != null)
            {
                // Check if the user is already part of the project
                var existingUserProject = _userContext.UserProjects
                    .SingleOrDefault(up => up.UID == currentUser.UID && up.PID == PID);

                if (existingUserProject == null)
                {
                    // User is not part of the project, add them to UserProjects
                    var newUserProject = new User_Project
                    {
                        UID = currentUser.UID,
                        PID = PID
                    };

                    _userContext.UserProjects.Add(newUserProject);
                    _userContext.SaveChanges();

                    TempData["Message"] = "Du har g�tt med i projektet!";
                    TempData["ErrorMessage"] = null;
                }
                else
                {
                    TempData["Message"] = null; 
                    TempData["ErrorMessage"] = "Du �r redan med i projektet!";
                }
            }
            else
            {
                return RedirectToAction("Error");
            }

            // Redirect back to the project details or wherever you want
            return RedirectToAction("Index", new { PID = PID });
        }


        
    }
}
