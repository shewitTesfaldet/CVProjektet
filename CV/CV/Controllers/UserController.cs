using Models;
using CV.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;

namespace CV.Controllers
{
    public class UserController : Controller
    {
        private UserContext _userContext;

        public UserController(UserContext userContext)
        {
            _userContext = userContext;
        }


        public IActionResult Add()
        {
            return View(new User());
        }

        [HttpPost]

        public IActionResult Add(User newUser)
        {
            if (ModelState.IsValid)
            {

                _userContext.Users.Add(newUser);
                _userContext.SaveChanges();
                ViewBag.Message = $"Registration successful! Welcome, {newUser.Firstname} {newUser.Lastname}.";

                return RedirectToAction("Add", "User");
            }

            else
            {
                return View(newUser);
            }


        }

        [Authorize]
        [HttpGet]
        public IActionResult Profile()
        {
            User user = new User();
/*            *//*            string Name = User.Identity.Name;
*/
        string Name = "user1";
        if (Name != null)
        {
            user = _userContext.Users.FirstOrDefault(x => x.Username.Equals(Name));
        }
        if (user.Username == null)
        {
/*             Om användaren inte finns, returnera NotFound
*/            return NotFound();
        }


        return View(user);
    }



        [Authorize]
        [HttpGet]
        public IActionResult Update()
        {
            User user = new User();
            /*            *//*            string Name = User.Identity.Name;
            */
            string Name = "user1";
            if (Name != null)
            {
                user = _userContext.Users.FirstOrDefault(x => x.Username.Equals(Name));
            }
            if (user.Username == null)
            {
                /*             Om användaren inte finns, returnera NotFound
                */
                return NotFound();
            }


            return View(user);
        }

        /*
                [Authorize]
                [HttpGet]
                public IActionResult Profile()
                {
                    User user = new User();
                    *//*            string Name = User.Identity.Name;
                    */

        /*HÅRDKODAD FÖR TEST*/
//        string Name = "user1";
//            if (Name != null)
//            {
//                user = _userContext.Users.FirstOrDefault(x => x.Username.Equals(Name));
//            }
//            if (user.Username == null)
//            {
//                // Om användaren inte finns, returnera NotFound
//                return NotFound();
//}


//return View(user);
//        }



        [HttpPost]
public IActionResult Update(User updatedUser)
{
    if (ModelState.IsValid)
    {
        string Name = "user1";

        var userToUpdate = _userContext.Users.FirstOrDefault(x => x.Username.Equals(Name));

        if (userToUpdate != null)
        {
            userToUpdate.Firstname = updatedUser.Firstname;
            userToUpdate.Lastname = updatedUser.Lastname;
            userToUpdate.Password = updatedUser.Password;
            userToUpdate.Adress = updatedUser.Adress;
            userToUpdate.Epost = updatedUser.Epost;

            if (!string.IsNullOrEmpty(updatedUser.ConfirmPassword) && updatedUser.Password == updatedUser.ConfirmPassword)
            {
                updatedUser.Password = updatedUser.ConfirmPassword;
            }


            _userContext.SaveChanges();


            return RedirectToAction("Profile");
        }
    }

    return View(updatedUser);
}



            /* {/*

                 if (ModelState.IsValid)
                 {
                     User existingUser = _userContext.Users.FirstOrDefault(x=> x.Equals(loginUser));

                     if (existingUser == null)
                     {
                         return NotFound();
                     }
                     existingUser.Password = updatedUser.Password;
                     existingUser.ConfirmPassword = updatedUser.ConfirmPassword;
                     existingUser.Firstname = updatedUser.Firstname;
                     existingUser.Lastname = updatedUser.Lastname;
                     existingUser.Epost = updatedUser.Epost;
                     existingUser.Adress = updatedUser.Adress;
                     existingUser.Privat = updatedUser.Privat;
                     existingUser.Username = updatedUser.Username;
                     // Spara ändringarna i databasen
                     _userContext.SaveChanges();

                     // Uppdatera ViewBag.Message med ett meddelande som bekräftar ändringarna
                     ViewBag.Message = "Profilen har uppdaterats";

                     // Återvänd till profilsidan
                     return View(existingUser);
                 }
                 else
                 {
                     // Om modellens tillstånd inte är giltigt, returnera vyn med fel
                     return View(updatedUser);
                 }
             }
         }*//*
             }*/
    }
    }