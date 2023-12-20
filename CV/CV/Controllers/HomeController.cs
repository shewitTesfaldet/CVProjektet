using CV.Models;
using CV.Models.Context;
using Microsoft.AspNetCore.Mvc;
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

        public IActionResult Index()
        {
            ViewBag.Meddelande = _userContext.Users.Select(x => x.Username);
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        //För sökrutan ska flyttas till UserContext
        public IActionResult Index(string söksträng) {

            List<User> users = new List<User>();
            if (!string.IsNullOrEmpty(söksträng))
            {
                users = _userContext.Users
                        .Where( x => x.Username.Contains(söksträng))
                        .ToList();
            }
            return View(users); 
        }
       
    }
}
