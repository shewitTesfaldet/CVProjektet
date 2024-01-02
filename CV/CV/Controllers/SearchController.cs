using CV.Models;
using Models;
using Microsoft.AspNetCore.Mvc;

namespace CV.Controllers
{
    public class SearchController : Controller
    {
        private readonly UserContext _context;
        public SearchController(UserContext context) 
        {
        
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Search(string söksträng)
        {

            ViewBag.Sök = "Resultatet för sökningen: '" + söksträng + "'";
            List<User> users = new List<User>();
            if (!string.IsNullOrEmpty(söksträng))
            {
                users = _context.Users
                        .Where(x => x.Username.Contains(söksträng) || x.Firstname.Contains(söksträng) || x.Lastname.Contains(söksträng))                 
                        .ToList();
            }                    
            return View(users);

        }
    }
}
