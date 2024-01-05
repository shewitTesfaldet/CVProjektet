
using CV.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using Models;

namespace CV.Controllers
{

    public class ChatController : Controller
    {
        private UserContext _userContext;

        private int LoggedInID;

        public ChatController(UserContext context)
        {
            _userContext = context;
        }

        [HttpGet]
        public IActionResult MessageBox(string message, int clickID, string getLogedOnUser)
        {
            List<Chat> AllMessages = new List<Chat>();
            //kod för att få ut vem man sökt på
            List<User> users = new List<User>();
            if (!string.IsNullOrEmpty(message))
            {
                users = _userContext.Users
                        .Where(x => x.Username.Contains(message) || x.Firstname.Contains(message) || x.Lastname.Contains(message))
                        .ToList();
            }
            ViewBag.users = users;
            if (clickID != 0)
            {
                AllMessages = GetMessages(clickID, getLogedOnUser);
                ViewBag.ClickedName = getClickedName(clickID);

            }
            return View(AllMessages);

        }

        [HttpGet]
        public List<Chat> GetMessages(int clickID, string getLogedOnUser)
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
                                  .Where(x => (x.SenderID.Equals(LoggedInID) && x.ReceiverID.Equals(clickID)) || (x.SenderID.Equals(clickID) && x.ReceiverID.Equals(LoggedInID)))
                                  .OrderBy(x => x.Date)
                                  .ToList();


            }

            return (AllMessages);
        }

        public string getClickedName(int clickedUID)
        {
            string? ClickedName = _userContext.Users
                                       .Where(x => x.UID.Equals(clickedUID))
                                       .Select(x => x.Username)
                                       .FirstOrDefault();

            return ClickedName;
        }
            
        [HttpPost]
        public IActionResult MessageBox(string clickedName, string med, string getLogedOnUser)
        {
            //HÅRDKODNING FÖR TESTNING
            getLogedOnUser = "user1";
            
            int ClickedUID = 0;
            if (clickedName != null)
            {
                ClickedUID = _userContext.Users
                     .Where(x => x.Username.Equals(clickedName))
                     .Select(x => x.UID)
                     .FirstOrDefault();

            }

            if (!string.IsNullOrEmpty(med))
            {
                SendMessageTo(ClickedUID, med, getLogedOnUser);
            }
            List <Chat> AllMessages = new List <Chat>();
            if (ClickedUID != 0)
            {
                AllMessages = GetMessages(ClickedUID, getLogedOnUser);
               ViewBag.ClickedName = getClickedName(ClickedUID);

            }
            return View(AllMessages);
        }

        public void SendMessageTo(int ClickedUID, string med, string getLogedOnUser) {

            if(getLogedOnUser != null)
            {
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


