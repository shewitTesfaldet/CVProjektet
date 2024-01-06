using Microsoft.AspNetCore.Mvc;
using Models;
using CV.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;

namespace CV.Controllers
{
    public class ProjectController : Controller
    {
        private UserContext _userContext;

        public ProjectController(UserContext userContext)
        {
            _userContext = userContext;
        }

        public IActionResult Index()
        {
            return View();
        }


        public IActionResult Projects()
        {
            List<Project> projects = _userContext.Projects.ToList();
            return View(projects);
        }


        public IActionResult SearchProject(string söksträng)
        {

            ViewBag.Sök = "Resultatet för sökningen: '" + söksträng + "'";
            List<Project> projects = new List<Project>();
            if (!string.IsNullOrEmpty(söksträng))
            {
                projects = _userContext.Projects
                        .Where(x => x.Title.Contains(söksträng))
                        .ToList();
            }


            return View("Projects", projects);

        }

        [Authorize]
        [HttpGet]
        public IActionResult AddProject()
        {
            return View(new Project());
        }


        [HttpPost]
        public IActionResult AddProject(Project project)
        {
            if (ModelState.IsValid)
            {
                // Get the username of the currently logged-in user
                var currentUsername = User.Identity.Name;

                // Find the user in the database based on the username
                var currentUser = _userContext.Users.SingleOrDefault(u => u.Username == currentUsername);

                if (currentUser != null)
                {
                    // Set the UserCreatedBy property
                    project.UserCreatedBy = currentUser.UID; // Assuming UID is the property representing the user's ID

                    // Add the project to the database
                    _userContext.Projects.Add(project);
                    _userContext.SaveChanges();

                    // Create a User_Project object and add it to the context
                    var userProject = new User_Project
                    {
                        UID = currentUser.UID,
                        PID = project.PID  // Assuming PID is the property representing the project's ID
                    };

                    _userContext.UserProjects.Add(userProject);
                    _userContext.SaveChanges();

                    ViewBag.Meddelande = "Projektet har lagts till framgångsrikt";

                    return View(project);
                }
                else
                {
                    // Handle the case where the user is not found
                    return RedirectToAction("Error");
                }
            }

            return View(project);
        }


        //[HttpPost]
        //public IActionResult AddProject(Project project)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        // Get the username of the currently logged-in user
        //        var currentUsername = User.Identity.Name;

        //        // Find the user in the database based on the username
        //        var currentUser = _userContext.Users.SingleOrDefault(u => u.Username == currentUsername);

        //        if (currentUser != null)
        //        {
        //            // Set the UserCreatedBy property
        //            project.UserCreatedBy = currentUser.UID; // Assuming UID is the property representing the user's ID

        //        }
        //        else
        //        {
        //            // Handle the case where the user is not found
        //            return RedirectToAction("Error");
        //        }

        //        // Add the project to the database
        //        _userContext.Projects.Add(project);

        //        _userContext.SaveChanges();

        //        ViewBag.Meddelande = "Projektet har lagts till framgångsrikt";

        //        return View(project);
        //    }

        //    return View(project);
        //}



        [Authorize]
        [HttpGet]
        public IActionResult EditProject(int PID)
        {
            Project projects = _userContext.Projects.Find(PID);
            return View(projects);
        }


        [HttpPost]
        public IActionResult EditProject(Project editedProject)
        {
            if (ModelState.IsValid)
            {
                // Get the username of the currently logged-in user
                var currentUsername = User.Identity.Name;

                // Find the user in the database based on the username
                var currentUser = _userContext.Users.SingleOrDefault(u => u.Username == currentUsername);

                if (currentUser != null)
                {
                    // Get the existing project from the database
                    var existingProject = _userContext.Projects.Find(editedProject.PID);

                    // Check if the current user has the permission to edit this project
                    if (existingProject?.UserCreatedBy == currentUser.UID)
                    {
                        // Update specific properties based on user input
                        existingProject.Title = editedProject.Title;
                        existingProject.Description = editedProject.Description;
                        existingProject.BeginDate = editedProject.BeginDate;
                        existingProject.EndDate = editedProject.EndDate;

                        // Save changes
                        _userContext.SaveChanges();

                        ViewBag.Meddelande = "Projektet har uppdaterats framgångsrikt";

                        return View(existingProject);
                    }
                    else
                    {
                        // Handle unauthorized edit attempt
                        ViewBag.ErrorMessage = "Detta projekt har du inte tillgång till att uppdatera";
                        return View("EditProject", existingProject);
                    }
                }
                else
                {
                    // Handle the case where the user is not found
                    return RedirectToAction("Error");
                }
            }

            // Handle invalid model state
            return View(editedProject);
        }


        [Authorize]
        [HttpGet]
        public IActionResult Delete(int PID)
        {
            Project projectToDelete = _userContext.Projects.Find(PID);

            if (projectToDelete != null)
            {
                // Check if the current user has the permission to delete this project
                var currentUsername = User.Identity.Name;
                var currentUser = _userContext.Users.SingleOrDefault(u => u.Username == currentUsername);

                if (currentUser != null && projectToDelete.UserCreatedBy == currentUser.UID)
                {
                    // Remove associated UserProjects manually
                    var associatedUserProjects = _userContext.UserProjects.Where(up => up.PID == PID);
                    _userContext.UserProjects.RemoveRange(associatedUserProjects);

                  
                    // Delete the project
                    _userContext.Entry(projectToDelete).State = EntityState.Deleted;
                    _userContext.SaveChanges();

                    return RedirectToAction("Projects", "Project");
                }
                else
                {
                    // Handle unauthorized delete attempt
                    ViewBag.ErrorMessage = "Du har inte tillåtelse att radera detta projekt";
                    return View("Projects", _userContext.Projects.ToList());
                }
            }

            return NotFound();
        }



        [HttpPost]
        public IActionResult Delete(Project project)
        {

            ViewBag.Meddelande = "Projektet har raderats framgångsrikt";
            _userContext.Projects.Remove(project);
            _userContext.SaveChanges();
            return RedirectToAction("Projects", "Project");

        }

        [HttpGet]
        public IActionResult DeltagarLista(int PID)
        {
            if (User.Identity.IsAuthenticated)
            {

                var data = (from user in _userContext.Users
                            join userProject in _userContext.UserProjects on user.UID equals userProject.UID into userProjectGroup
                            from userProject in userProjectGroup.DefaultIfEmpty()
                            join project in _userContext.Projects on userProject.PID equals project.PID into projectGroup
                            from project in projectGroup.DefaultIfEmpty()
                            where project != null && project.PID == PID
                            select new DeltagarListaViewModel
                            {
                                UID = user.UID,
                                Firstname = user.Firstname,
                                Lastname = user.Lastname,
                                Privat = user.Privat,
                                ProjectDescription = (project == null ? null : project.Description)
                            }).ToList();

                return View(data);
            }

            else
            {

                var data = (from user in _userContext.Users
                            join userProject in _userContext.UserProjects on user.UID equals userProject.UID into userProjectGroup
                            from userProject in userProjectGroup.DefaultIfEmpty()
                            join project in _userContext.Projects on userProject.PID equals project.PID into projectGroup
                            from project in projectGroup.DefaultIfEmpty()
                            where user.Privat == false && project != null && project.PID == PID
                            select new DeltagarListaViewModel
                            {
                                UID = user.UID,
                                Firstname = user.Firstname,
                                Lastname = user.Lastname,
                                Privat = user.Privat,
                                ProjectDescription = (project == null ? null : project.Description)
                            }).ToList();

                return View(data);
            }
        }




    }
}
        

