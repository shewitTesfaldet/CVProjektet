using CV.Models;
using Microsoft.AspNetCore.Mvc;
using Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace CV.Controllers
{
    public class MessageService
    {
        private readonly UserContext _userContext;

        public MessageService(UserContext userContext)
        {
            _userContext = userContext;
        }

        public bool HasUnreadMessages(string currentUsername)
        {
            if (currentUsername != null)
            {
                var currentUser = _userContext.Users.SingleOrDefault(u => u.Username == currentUsername);

                if (currentUser != null)
                {
                    try
                    {
                        // Retrieve unread messages for the specific user where 'Read' is false
                        return _userContext.Chats
                            .Any(chat => chat.ReceiverID == currentUser.UID && chat.Read == false);
                    }
                    catch (Exception ex)
                    {
                        // Log or print the exception details
                        Console.WriteLine($"Exception: {ex.Message}");
                    }
                }
            }

            return false;
        }
    }




}
