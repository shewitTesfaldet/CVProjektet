using Models;
using CV.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Authorization;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CV.Controllers
{
    public class CV_Controller : Controller
    {

        private UserContext _userContext;

        private Project _project;
        private MessageService _messageService;


        public CV_Controller(UserContext userContext, MessageService messageService)
        {
            _userContext = userContext;
            _messageService = messageService;
        }




        public IActionResult ResumeHistory(int UID)
        {
            var currentUsername = User.Identity.Name;
            var hasUnreadMessages = _messageService.HasUnreadMessages(currentUsername);
            ViewBag.HasUnreadMessages = hasUnreadMessages;
            List<CV_> cv = _userContext.CV_s.ToList();

            var cvInfo = (from user in _userContext.Users
                          where user.UID == UID
                          select new { user.Firstname, user.Lastname, user.Epost, user.Privat });
            ViewBag.cvInfo = cvInfo;

            return View("EditResume", cv);
        }





        public IActionResult SearchCV(string söksträng)
        {

            ViewBag.Sök = "Resultatet för sökningen: '" + söksträng + "'";
            List<CV_> cv = new List<CV_>();
            if (!string.IsNullOrEmpty(söksträng))
            {
                cv = _userContext.CV_s
                        .Where(x => x.User.Firstname.Contains(söksträng))
                        .ToList();

                if (cv.IsNullOrEmpty())
                {
                    cv = _userContext.CV_s
                            .Where(x => x.User.Lastname.Contains(söksträng))
                            .ToList();
                }
            }

            return View("EditResume", cv);
        }



        public IActionResult AddResume(int UID)
        {
            var currentUsername = User.Identity.Name;
            var hasUnreadMessages = _messageService.HasUnreadMessages(currentUsername);
            ViewBag.HasUnreadMessages = hasUnreadMessages;
            int CVID = _userContext.CV_s.Where(x => x.UID.Equals(UID)).Select(x => x.CID).FirstOrDefault();

            if (UID == 0)
            {
                var currentUser = (from u in _userContext.Users
                                   where u.Firstname == User.Identity.Name
                                   select u.UID);
                UID = currentUser.First();
            }

            /*            ProfilBild
			*/
            ViewBag.UID = UID;
            var profilePicture = (from cv in _userContext.CV_s
                                  where cv.UID == UID
                                  select cv.Picture).FirstOrDefault();

            ViewBag.ProfilePicture = profilePicture;





            /*För- och efternamn*/
            var userFullnameList = (from id in _userContext.Users
                                    select id.UID).ToList();

            for (int fn = 0; fn < userFullnameList.Count(); fn++)
            {
                int fullnameElement = userFullnameList.ElementAt(fn);
                if (UID == fullnameElement)
                {
                    var fullname = (from user in _userContext.Users
                                    where user.UID == UID
                                    select new { user.Firstname, user.Lastname });

                    ViewBag.Fullname = fullname;
                }
                else
                {
                    ViewBag.noName = "Saknar Namn";
                }
            }
            /*Utbildning*/

            List<int> educationID = _userContext.CV_Educations.Where(x => x.CID.Equals(CVID)).Select(x => x.EdID).ToList();
            List<Education> educationList = new List<Education>();
            if (educationID.Count != 0)
            {
                foreach (int i in educationID)
                {
                    Education oneEducation = _userContext.Education.Where(x => x.EdID.Equals(i)).FirstOrDefault();
                    educationList.Add(oneEducation);
                }
                ViewBag.Education = educationList;

            }
            else
            {
                ViewBag.EducationMessage = "Saknar Utbildning";
            }


            /*            Kontaktinformation*/

            var userContactList = (from id in _userContext.Users
                                   select id.UID).ToList();

            for (int contid = 0; contid < userContactList.Count(); contid++)
            {
                int contact = userContactList.ElementAt(contid);
                if (UID == contact)
                {

                    var contactinfo = (from user in _userContext.Users
                                       where user.UID == UID
                                       select new { user.Epost, user.Adress }).ToList();

                    ViewBag.Kontaktinfo = contactinfo;

                }
            }
            List<int> CompID = _userContext.CV_Competences.Where(x => x.CID.Equals(CVID)).Select(x => x.CompID).ToList();
            List<Competence> competenceList = new List<Competence>();
            if (CompID.Count != 0)
            {
                foreach (int i in CompID)
                {
                    Competence oneCompetence = _userContext.Competence.Where(x => x.CompID.Equals(i)).FirstOrDefault();
                    competenceList.Add(oneCompetence);
                }
                ViewBag.Competence = competenceList;

            }
            else
            {

                ViewBag.CompetenceMessage = "Saknar kompetenser";

            }


            /*Projekt*/

            var userList = (from id in _userContext.UserProjects
                            select id.UID).ToList();

            for (int i = 0; i < userList.Count(); i++)
            {
                int proj = userList.ElementAt(i);
                if (UID == proj)
                {
                    var project = (from user in _userContext.Users
                                   where user.UID == UID
                                   join u in _userContext.UserProjects on user.UID equals u.UID into userProjectGroup
                                   from u in userProjectGroup.DefaultIfEmpty()

                                   join p in _userContext.Projects on u.PID equals p.PID into projectGroup
                                   from p in projectGroup.DefaultIfEmpty()

                                   select new
                                   {
                                       Title = p == null ? null : p.Title,
                                       Description = p == null ? null : p.Description,
                                       p.BeginDate,
                                       p.EndDate
                                   }).ToList();

                    ViewBag.Project = project;
                }
                else
                {
                    ViewBag.ProjectMessage = "Saknar Projekt";
                }
            }


            //Erfarenheter
            List<Experience> ExperienceList = _userContext.Experience.Where(x => x.CID.Equals(CVID)).ToList();
            if (ExperienceList.Count != 0)
            {
                ViewBag.Experience = ExperienceList;

            }
            else
            {
                ViewBag.ExperienceMessage = "Saknar Erfarenheter";
            }

            return View(new CV_());

        }

        #region Experience

        [Authorize]
        [HttpGet]
        public IActionResult AddExperience()
        {
            string Name = User.Identity.Name;

            int LogedInID = _userContext.Users
                             .Where(x => x.Username.Equals(Name))
                             .Select(x => x.UID)
                             .FirstOrDefault();
            int CVID = _userContext.CV_s.Where(x => x.UID.Equals(LogedInID)).Select(x => x.CID).FirstOrDefault();

            List<int> ExID = _userContext.Experience.Where(x => x.CID.Equals(CVID)).Select(x => x.EID).ToList();
            List<Experience> Experiences = new List<Experience>();
            foreach (int ID in ExID)
            {
                Experience oneExperience = _userContext.Experience.Where(x => x.EID.Equals(ID)).FirstOrDefault();
                if (oneExperience != null)
                {
                    Experiences.Add(oneExperience);

                }

            }
            ViewBag.Experiences = Experiences;

            return View(new Experience());
        }

        [HttpPost]
        public async Task<IActionResult> AddExperience(Experience oneExperience)
        {

            if (ModelState.IsValid)
            {
                string Name = User.Identity.Name;

                int LogedInID = _userContext.Users
                                 .Where(x => x.Username.Equals(Name))
                                 .Select(x => x.UID)
                                 .FirstOrDefault();
                int CVID = _userContext.CV_s.Where(x => x.UID.Equals(LogedInID)).Select(x => x.CID).FirstOrDefault();

                if (LogedInID != 0)
                {
                    oneExperience.CID = CVID;
                    _userContext.Experience.Add(oneExperience);
                    await _userContext.SaveChangesAsync();


                    ViewBag.Meddelande = "Kompetensen har lagts till framgångsrikt";

                }

            }
            else
            {
                return RedirectToAction("Error");
            }

            return RedirectToAction("AddExperience", "CV_");
        }

        [Authorize]
        [HttpGet]
        public IActionResult EditExperience(int EID)
        {
            Experience Experience = _userContext.Experience.Where(x => x.EID.Equals(EID)).FirstOrDefault();
            return View(Experience);
        }

        [HttpPost]
        public ActionResult EditExperience(Experience model)
        {
            if (ModelState.IsValid)
            {
                //Hämta den befintliga kompetensen från databasen
                /*                Competence existingCompetence = _userContext.Competence.Where(x => x.CompID.Equals(CompID)).FirstOrDefault();
                */
                Experience existingCompetence = _userContext.Experience.Find(model.EID);

                if (existingCompetence != null)
                {
                    //Uppdatera beskrivningen av den befintliga kompetensen
                    existingCompetence.Description = model.Description;

                    //Spara ändringarna till databasen
                    _userContext.SaveChanges();

                    //Skicka ett meddelande till vyn för att informera användaren om att ändringarna har sparats
                    ViewBag.Meddelande = "Kompetensen har uppdaterats framgångsrikt.";
                    return View(existingCompetence);
                }
                else
                {
                    //Om kompetensen inte hittades, skicka ett felmeddelande till vyn
                    ViewBag.Meddelande = "Kompetensen kunde inte hittas.";
                }
            }

            // Återgå till samma vy (eller en annan vy beroende på dina behov)
            return View();
        }

        #endregion
        #region Education
        [HttpGet]
        public IActionResult AddEducation()
        {
            string Name = User.Identity.Name;

            int LogedInID = _userContext.Users
                             .Where(x => x.Username.Equals(Name))
                             .Select(x => x.UID)
                             .FirstOrDefault();
            int CVID = _userContext.CV_s.Where(x => x.UID.Equals(LogedInID)).Select(x => x.CID).FirstOrDefault();

            List<int> EducationID = _userContext.CV_Educations.Where(x => x.CID.Equals(CVID)).Select(x => x.EdID).ToList();
            List<Education> Educations = new List<Education>();
            foreach (int ID in EducationID)
            {
                Education oneEducation = _userContext.Education.Where(x => x.EdID.Equals(ID)).FirstOrDefault();
                if (oneEducation != null)
                {
                    Educations.Add(oneEducation);

                }

            }
            ViewBag.Educations = Educations;

            return View(new Education());
        }

        [HttpPost]
        public async Task<IActionResult> AddEducation(Education newEducation)
        {

            if (ModelState.IsValid)
            {
                string Name = User.Identity.Name;

                int LogedInID = _userContext.Users
                                 .Where(x => x.Username.Equals(Name))
                                 .Select(x => x.UID)
                                 .FirstOrDefault();
                int CVID = _userContext.CV_s.Where(x => x.UID.Equals(LogedInID)).Select(x => x.CID).FirstOrDefault();

                if (LogedInID != 0)
                {
                    _userContext.Education.Add(newEducation);
                    await _userContext.SaveChangesAsync();

                    int newEdID = newEducation.EdID;

                    CV_Education CV_ED = new CV_Education();
                    CV_ED.CID = CVID;
                    CV_ED.EdID = newEdID;
                    _userContext.CV_Educations.Add(CV_ED);
                    await _userContext.SaveChangesAsync();

                    ViewBag.Meddelande = "Utbildningen har lagts till framgångsrikt";

                }

            }
            else
            {
                return RedirectToAction("Error");
            }

            return RedirectToAction("AddEducation", "CV_");
        }

        [HttpGet]
        public IActionResult EditEducation(int edID)
        {
            Education education = _userContext.Education.Where(x => x.EdID.Equals(edID)).FirstOrDefault();
            return View(education);
        }

        [HttpPost]
        public ActionResult EditEducation(Education model)
        {
            if (ModelState.IsValid)
            {
                //Hämta den befintliga kompetensen från databasen
                /*                Competence existingCompetence = _userContext.Competence.Where(x => x.CompID.Equals(CompID)).FirstOrDefault();
                */
                Education existingCompetence = _userContext.Education.Find(model.EdID);

                if (existingCompetence != null)
                {
                    //Uppdatera beskrivningen av den befintliga kompetensen
                    existingCompetence.Description = model.Description;

                    //Spara ändringarna till databasen
                    _userContext.SaveChanges();

                    //Skicka ett meddelande till vyn för att informera användaren om att ändringarna har sparats
                    ViewBag.Meddelande = "Kompetensen har uppdaterats framgångsrikt.";
                    return View(existingCompetence);
                }
                else
                {
                    //Om kompetensen inte hittades, skicka ett felmeddelande till vyn
                    ViewBag.Meddelande = "Kompetensen kunde inte hittas.";
                }
            }

            // Återgå till samma vy (eller en annan vy beroende på dina behov)
            return View();
        }

        #endregion
        #region Competence
        [Authorize]
        [HttpGet]
        public IActionResult AddCompetence()
        {
            string Name = User.Identity.Name;

            int LogedInID = _userContext.Users
                             .Where(x => x.Username.Equals(Name))
                             .Select(x => x.UID)
                             .FirstOrDefault();
            int CVID = _userContext.CV_s.Where(x => x.UID.Equals(LogedInID)).Select(x => x.CID).FirstOrDefault();

            List<int> ComptenceID = _userContext.CV_Competences.Where(x => x.CID.Equals(CVID)).Select(x => x.CompID).ToList();
            List<Competence> Competences = new List<Competence>();
            foreach (int ID in ComptenceID)
            {
                Competence oneCompetence = _userContext.Competence.Where(x => x.CompID.Equals(ID)).FirstOrDefault();
                if (oneCompetence != null)
                {
                    Competences.Add(oneCompetence);

                }

            }
            ViewBag.Competences = Competences;

            return View(new Competence());
        }

        [HttpPost]
        public async Task<IActionResult> AddCompetence(Competence newCompetence)
        {

            if (ModelState.IsValid)
            {
                string Name = User.Identity.Name;

                int LogedInID = _userContext.Users
                                 .Where(x => x.Username.Equals(Name))
                                 .Select(x => x.UID)
                                 .FirstOrDefault();
                int CVID = _userContext.CV_s.Where(x => x.UID.Equals(LogedInID)).Select(x => x.CID).FirstOrDefault();

                if (LogedInID != 0)
                {
                    _userContext.Competence.Add(newCompetence);
                    await _userContext.SaveChangesAsync();

                    int newCompID = newCompetence.CompID;

                    CV_Competence CV_CO = new CV_Competence();
                    CV_CO.CID = CVID;
                    CV_CO.CompID = newCompID;
                    _userContext.CV_Competences.Add(CV_CO);
                    await _userContext.SaveChangesAsync();

                    ViewBag.Meddelande = "Kompetensen har lagts till framgångsrikt";

                }

            }
            else
            {
                return RedirectToAction("Error");
            }

            return RedirectToAction("AddCompetence", "CV_");
        }




        [Authorize]
        [HttpGet]
        public IActionResult EditCompetence(int CompID)
        {
            Competence competence = _userContext.Competence.Where(x => x.CompID.Equals(CompID)).FirstOrDefault();
            return View(competence);
        }

        [HttpPost]
        public ActionResult EditCompetence(Competence model)
        {
            if (ModelState.IsValid)
            {
                //Hämta den befintliga kompetensen från databasen
                /*                Competence existingCompetence = _userContext.Competence.Where(x => x.CompID.Equals(CompID)).FirstOrDefault();
                */
                Competence existingCompetence = _userContext.Competence.Find(model.CompID);

                if (existingCompetence != null)
                {
                    //Uppdatera beskrivningen av den befintliga kompetensen
                    existingCompetence.Description = model.Description;

                    //Spara ändringarna till databasen
                    _userContext.SaveChanges();

                    //Skicka ett meddelande till vyn för att informera användaren om att ändringarna har sparats
                    ViewBag.Meddelande = "Kompetensen har uppdaterats framgångsrikt.";
                    return View(existingCompetence);
                }
                else
                {
                    //Om kompetensen inte hittades, skicka ett felmeddelande till vyn
                    ViewBag.Meddelande = "Kompetensen kunde inte hittas.";
                }
            }

            // Återgå till samma vy (eller en annan vy beroende på dina behov)
            return View();
        }
        #endregion


    }







}

