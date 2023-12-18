using CV.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CV.Controllers
{
    public class HomeController : Controller
    {

        public HomeController()
        {
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

       
    }
}
