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


        public IActionResult Index()
        {

            List<User> produktList = _userContext.Users.ToList();
            Console.WriteLine($"Antal användare: {produktList.Count}");
            ViewBag.Meddelande = "Antal i listan";
            return View(produktList);


        }
      

    }
}
