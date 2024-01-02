
using CV.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using Models;

namespace CV.Controllers
{
    // ChatController.cs

    public class ChatController : Controller
    {
        private UserContext _userContext;


        public ChatController(UserContext context)
        {
            _userContext = context;
        }

		[HttpGet]
		public IActionResult MessageBox()
		{
			return View();
		}


		[HttpPost]
        public IActionResult MessageBox(string message, int UID, string med)
        {

            List<User> users = new List<User>();
            if (!string.IsNullOrEmpty(message))
            {
                users = _userContext.Users
                        .Where(x => x.Username.Contains(message) || x.Firstname.Contains(message) || x.Lastname.Contains(message))
                        .ToList();
            }
            ViewBag.users = users;
            //kod för att få ut vem man valt

            if (!string.IsNullOrEmpty(med))
            {
                SendMessageTo(users, UID, med);

            }
            return View();
              }

        public void SendMessageTo(List<User> userlist, int UID, string med) {

			User sendTo = _userContext.Users.FirstOrDefault(x => x.UID.Equals(UID));

			/*if (users[i].Equals(sendTo))
            {*/
			    Chat chat = new Chat();
                chat.Text = med;
                chat.Date = DateTime.Now;
                chat.Read = false;
                chat.UID = sendTo.UID;

                _userContext.Chats.Add(chat);
                _userContext.SaveChanges();
            /*}*/
        }
    }
}


