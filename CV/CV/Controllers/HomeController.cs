using CV.Models;
using CV.Models.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;

namespace CV.Controllers
{
    public class HomeController : Controller
    {
        private UserContext _userContext;
        public HomeController(UserContext usercontext)
        {
            _userContext = usercontext;

        }


        public IActionResult Privacy()
        {
            return View();
        }
        
        //F�r s�krutan ska flyttas till UserContext
        //public IActionResult Index(string s�kstr�ng) {

        //    List<User> users = new List<User>();
        //    if (!string.IsNullOrEmpty(s�kstr�ng))
        //    {
        //        users = _userContext.Users
        //                .Where( x => x.Username.Contains(s�kstr�ng))
        //                .ToList();
        //    }
        //    return View(users); 

        //}

        public IActionResult Index()
        {

            List<User> produktList = _userContext.Users.ToList();
            Console.WriteLine($"Antal anv�ndare: {produktList.Count}");
            ViewBag.Meddelande = "Antal i listan";
            return View(produktList);


        }


    }
}
