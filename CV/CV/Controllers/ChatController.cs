
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

        private int LoggedInID;
        private int ClickedUID;

        public ChatController(UserContext context)
        {
            _userContext = context;
        }

		[HttpGet]
		public IActionResult MessageBox(string message, int clickID, string getLogedOnUser)
		{
            //kod för att få ut vem man sökt på
            List<User> users = new List<User>();
            if (!string.IsNullOrEmpty(message))
            {
                users = _userContext.Users
                        .Where(x => x.Username.Contains(message) || x.Firstname.Contains(message) || x.Lastname.Contains(message))
                        .ToList();
            }
            ViewBag.users = users;

            return View(GetMessages(clickID, getLogedOnUser));
		}

        [HttpGet]
        public List<Chat> GetMessages(int UID, string getLogedOnUser) 
        {
            List<Chat> AllMessages = new List<Chat>();

            if (getLogedOnUser != null)
            {
                //HÅRDKODNING FÖR TESTNING
                getLogedOnUser = "user1";

                //Kod för att hämta meddelanden mellan inloggad och den man tryckt på 
                int? LoggedInID = _userContext.Users
                      .Where(x => x.Username.Equals(getLogedOnUser))
                      .Select(x => x.UID)
                      .FirstOrDefault();
                ViewBag.LoggedInID = LoggedInID;


                //Hämtar alla meddelande där den inloggade har sender och receiver id samt där den man tryckt på har sender och receiver id 
                AllMessages = _userContext.Chats
                                  .Where(x => (x.SenderID.Equals(LoggedInID) && x.ReceiverID.Equals(UID)) || (x.SenderID.Equals(UID) && x.ReceiverID.Equals(LoggedInID)))
                                  .OrderBy(x => x.Date)
                                  .ToList();

/*                ViewBag.AllMessages = AllMessages;
*/
            }

            return AllMessages;
        }


        [HttpPost]
        public IActionResult MessageBox( int UID, string med, string getLogedOnUser)
        {
            ClickedUID = UID;
            //HÅRDKODNING FÖR TESTNING
            getLogedOnUser = "user1";
       
            if (ClickedUID != 0)
            {
                string? ClickedUser =    _userContext.Users
                                       .Where(x => x.UID.Equals(ClickedUID))
                                       .Select(x => x.Username)
                                       .FirstOrDefault();
               
                ViewBag.ClickedName = ClickedUser;
                ViewBag.ClickedID = UID;
            }
           
            if (!string.IsNullOrEmpty(med))
            {
                SendMessageTo(ClickedUID, med, getLogedOnUser);
            }
          
            return View();
        }

        public void SendMessageTo(int ClickedUID, string med, string getLogedOnUser) {

            if(getLogedOnUser != null)
            {
                //Kod för att hämta meddelanden mellan inloggad och den man tryckt på 
                       LoggedInID = _userContext.Users
                      .Where(x => x.Username.Equals(getLogedOnUser))
                      .Select(x => x.UID)
                      .FirstOrDefault();
            }

            //Transaction för att säkerställa att inget läggs in om det inte går igenom helt
            using (var dbContextTransaction = _userContext.Database.BeginTransaction())
            {
                try
                {
                    Chat chat = new Chat();
                    chat.Text = med;
                    chat.Date = DateTime.Now;
                    chat.Read = false;
                    chat.SenderID = LoggedInID; /*OBS HÅRDKODAT BÖR ÄNDRAS*/
                    chat.ReceiverID = ClickedUID;

                    _userContext.Chats.Add(chat);
                    _userContext.SaveChanges();
                    dbContextTransaction.Commit();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    dbContextTransaction.Rollback();
                }
            }

            
            
        }
    }
}


