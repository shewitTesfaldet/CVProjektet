
using CV.Models;
using CV.Models.Context;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace CV.Controllers
{
    // ChatController.cs

        public class ChatController : Controller
        {
        private UserContext _userContext;

        //kod för getlogged on user
        private string getLogedOnUser;
        public ChatController(UserContext context)
        {
            _userContext = context;
        }
            public IActionResult Index()
            {

            ViewBag.Meddelanden = _userContext.Chats.Where(x => x.UID.Equals(1))
                        .ToList();
            return View(new Chat());
            }

            [HttpPost]
            public IActionResult SendMessage(Chat message)
            {
                _userContext.Chats.Add(message);
                return RedirectToAction("Index");
            }
        }
    }


