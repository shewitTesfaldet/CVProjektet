
using CV.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using Models;
using Microsoft.AspNetCore.Authorization;
using System;
using System.IO;

namespace CV.Controllers
{
    public class ChatController : Controller
    {
        private UserContext _userContext;
        private MessageService _messageService;
        private int LoggedInID;
        private readonly IWebHostEnvironment _env;


        public ChatController(UserContext context, MessageService messageService, IWebHostEnvironment env)
        {
            _userContext = context;
            _messageService = messageService;
            _env = env;
        }


        [HttpGet]
        public IActionResult MessageBox(int clickID, string message, string getLogedOnUser, string anonym)
        {
            Response.Cookies.Append("clickID", clickID.ToString(), new CookieOptions { HttpOnly = true });

            string Name = User.Identity.Name;

            int LogedInID = _userContext.Users
                             .Where(x => x.Username.Equals(Name))
                             .Select(x => x.UID)
                             .FirstOrDefault();



            var profilePicture = (from cv in _userContext.CV_s
                                  where cv.UID == LogedInID
                                  select cv.Picture).FirstOrDefault();

            var ClickedProfilePicture = (from cv in _userContext.CV_s
                                         where cv.UID == clickID
                                         select cv.Picture).FirstOrDefault();

            ViewBag.ProfilePicture = Path.Combine(_env.WebRootPath, "Pictures", profilePicture); 
            if(ClickedProfilePicture == null)
            {
                ClickedProfilePicture = "no_profile_picture.jpg";
            }
            ViewBag.ClickedProfilePicture = Path.Combine(_env.WebRootPath, "Pictures", ClickedProfilePicture);






            getLogedOnUser = User.Identity.Name;
            if (getLogedOnUser != null)
            {
                ViewBag.getLogedOnUser = getLogedOnUser;
            }
            else
            {
                ViewBag.getLogedOnUser = anonym;
            }

            if (anonym != null)
            {
                ViewBag.anonym = anonym;
                User anonymUser = new User();
                anonymUser.Username = anonym;
                anonymUser.Firstname = "Anonym";
                anonymUser.Lastname = "Anonym";
                anonymUser.Epost = anonym + "@hotmail.com";
                anonymUser.Password = anonym + "@hotmail.com";
                anonymUser.ConfirmPassword = anonym + "H24!";
                _userContext.Users.Add(anonymUser);
                _userContext.SaveChanges();

            }

            List<Chat> AllMessages = new List<Chat>();
            //kod för att få ut vem man sökt på
            List<User> users = new List<User>();
            List<string> userpictures = new List<string>();
            if (!string.IsNullOrEmpty(message))
            {
                users = _userContext.Users
                        .Where(x => x.Username.Contains(message) || x.Firstname.Contains(message) || x.Lastname.Contains(message))
                        .ToList();
            }
            ViewBag.users = users;



            if (clickID == 0)
            {
                if (Request.Cookies["clickID"] != null && !string.IsNullOrEmpty(Request.Cookies["clickID"]))
                {
                    if (int.TryParse(Request.Cookies["clickID"], out int parsedClickID))
                    {
                        clickID = parsedClickID;
                    }
                    else
                    {
                        // Felaktigt värde i cookien som inte kan konverteras till en int
                        // Ta lämpliga åtgärder, t.ex. logga eller hantera felet på annat sätt
                    }
                }
                else
                {
                    // Cookien finns inte eller är tom
                    // Ta lämpliga åtgärder, t.ex. logga eller hantera felet på annat sätt
                }
            }

            if (clickID != 0)
            {

                AllMessages = GetMessages(clickID, getLogedOnUser);
                ViewBag.ClickedName = getClickedName(clickID);
            }

            var currentUsername = User.Identity.Name;
            var hasUnreadMessages = _messageService.HasUnreadMessages(currentUsername);
            ViewBag.HasUnreadMessages = hasUnreadMessages;

            return View(AllMessages);

        }

        [HttpGet]
        public List<Chat> GetMessages(int clickID, string getLogedOnUser)
        {
            List<Chat> AllMessages = new List<Chat>();
            if (getLogedOnUser == null)
            {
                getLogedOnUser = User.Identity.Name;

            }

            if (getLogedOnUser != null)
            {

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

            if (string.IsNullOrEmpty(getLogedOnUser))
            {
                getLogedOnUser = User.Identity.Name;
                ViewBag.getLogedOnUser = getLogedOnUser;

            }

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
            List<Chat> AllMessages = new List<Chat>();
            if (ClickedUID != 0)
            {
                AllMessages = GetMessages(ClickedUID, getLogedOnUser);
                ViewBag.ClickedName = getClickedName(ClickedUID);
                ViewBag.anonym = getLogedOnUser;

            }
            return View(AllMessages);
        }

        public void SendMessageTo(int ClickedUID, string med, string getLogedOnUser)
        {

            if (getLogedOnUser != null)
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
                    chat.ReceiverID = ClickedUID;
                    chat.SenderID = LoggedInID;
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

        [HttpPost]
        public IActionResult MarkMessageAsRead(int messageId)
        {

            var message = _userContext.Chats.FirstOrDefault(m => m.MID == messageId);

            if (message != null && !message.Read.HasValue || !message.Read.Value)
            {
                message.Read = true;
                _userContext.SaveChanges();
            }


            return RedirectToAction("MessageBox");
        }







    }
}


