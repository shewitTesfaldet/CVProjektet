using Microsoft.AspNetCore.Mvc;
using CV.Models.Context;
using CV.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

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


        [HttpGet]
        public IActionResult AddProject()
        {
            return View(new Project());

        }


        [HttpPost]

        public IActionResult AddProject(Project project)
        {

            ViewBag.Meddelande = "Projektet har lagts till framgångsrikt";
            _userContext.Projects.Add(project);
            _userContext.SaveChanges();

            return View(project);


        }


        [HttpGet]
        public IActionResult EditProject(int PID)
        {
            Project projects = _userContext.Projects.Find(PID);
            return View(projects);
        }


        [HttpPost]
        public IActionResult EditProject(Project project)
        {


            _userContext.Projects.Update(project);

            _userContext.SaveChanges();
            return RedirectToAction("Projects", "Project");

        }


        [HttpGet]
        public IActionResult Delete(int PID)
        {
            Project projectToDelete = _userContext.Projects.Find(PID);

            if (projectToDelete != null)
            {
                _userContext.Entry(projectToDelete).State = EntityState.Deleted;
                _userContext.SaveChanges();

                return RedirectToAction("Projects", "Project");
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


    }
}
        

