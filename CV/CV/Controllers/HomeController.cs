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

namespace CV.Controllers
{
    public class HomeController : Controller
    {
        private UserContext _userContext;
        public HomeController(UserContext usercontext)
        {
            _userContext = usercontext;
        }


      
        [Authorize]
        public  IActionResult Index()
        {
            //Hämtar alla användare med utbildning i en lista

            var data =  (from user in  _userContext.Users
                        join cv in _userContext.CV_s on user.UID equals cv.UID into cvGroup
                        from cv in cvGroup.DefaultIfEmpty()
                        join cvEdu in _userContext.CV_Educations on cv.CID equals cvEdu.CID into cvEduGroup
                        from cvEdu in cvEduGroup.DefaultIfEmpty()
                        join edu in _userContext.Education on cvEdu.EdID equals edu.EdID into eduGroup
                        from edu in eduGroup.DefaultIfEmpty()
                        select new { user.UID, user.Firstname, user.Lastname, user.Username, user.Privat, Description = (edu == null ? null : edu.Description) }).ToList();
            //sorterar listan utofrån de senaste inlaggda och de 5 första.


            var result = data.GroupBy(x => x.UID)
                             .Select(g => g.First())
                             .OrderByDescending(x => x.UID)
                             .Take(5)
                             .ToList();

            ViewBag.Result = result;
            //Hämtar projekt utifrån användare och sorterar de senaste
            var Presult =  (from user in _userContext.Users
                           join userProject in _userContext.UserProjects on user.UID equals userProject.UID
                           join project in _userContext.Projects on userProject.PID equals project.PID
                           orderby project.PID descending
                           select new { user.Username, project.Title, project.Description })
             .Take(5)
             .ToList();

            ViewBag.Presult = Presult;



            return View();

        }

        
    }
}
