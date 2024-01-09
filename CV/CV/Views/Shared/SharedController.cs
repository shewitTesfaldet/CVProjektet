using Microsoft.AspNetCore.Mvc;
using CV.Models;
using Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Security.Cryptography;
using System.Net;
using Microsoft.AspNetCore.Authorization;

namespace CV.Views.Shared
{
    public class SharedController : Controller
    {
        private UserContext _userContext;
        public SharedController(UserContext usercontext)
        {
            _userContext = usercontext;
        }



        public void MessageBox()
        {
            var currentUsername = User.Identity.Name;


            if (currentUsername != null)
            {
                try
                {
                    int? LoggedInID = _userContext.Users
                     .Where(x => x.Username.Equals(currentUsername))
                     .Select(x => x.UID)
                     .FirstOrDefault();


                    // Retrieve unread messages for the specific user where 'Read' is false
                    bool hasUnreadMessages = _userContext.Chats
                        .Any(chat => chat.ReceiverID == LoggedInID && chat.Read == false);


                    // Pass the information to the ViewBag
                    ViewBag.HasUnreadMessages = hasUnreadMessages;
                }
                catch (Exception ex)
                {
                    // Log or print the exception details
                    Console.WriteLine($"Exception: {ex.Message}");
                }
            }



        }
    }

}


