using CV.Models;
using CV.Models.Context;
using Microsoft.AspNetCore.Mvc;
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

        public IActionResult Index()
        {
            ViewBag.Meddelande = _userContext.Users.Select(x => x.Username);
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

       
    }
}
