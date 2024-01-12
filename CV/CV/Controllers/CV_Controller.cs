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

namespace CV.Controllers
{
    public class CV_Controller : Controller
    {

        private UserContext _userContext;

        private Project _project;


        public CV_Controller(UserContext userContext)
        {
            _userContext = userContext;

        }




        public IActionResult ResumeHistory(int UID)
        {
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
			var pictureList = (from id in _userContext.CV_s
                               select id.CID).ToList();


            for (int pid = 0; pid < pictureList.Count(); pid++)
            {

                int expIdList = pictureList.ElementAt(pid);

                if (UID == expIdList)
                {

                    var profilePicture = (from cv in _userContext.CV_s
                                          where cv.UID == UID
                                          select cv.Picture).FirstOrDefault();

                    ViewBag.ProfilePicture = profilePicture;


                }



            }

            if (!pictureList.Contains(UID))
            {
                string filePath = "C:\\Users\\Admin\\OneDrive\\Dokument\\Webbsystem(.NET)\\CVProjekt\\CV\\CV\\wwwroot\\Pictures\\" + ViewBag.noProfilePicture + "";


                string FilePathExists = Path.Combine(filePath);

                if (!System.IO.File.Exists(FilePathExists))
                {
                    ViewBag.noProfilePicture = "no_profile_picture.jpg";
                }
            }

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
            var userListEdu = (from id in _userContext.CV_Educations
                               select id.CID).ToList();


            for (int eid = 0; eid < userListEdu.Count(); eid++)
            {

                int eduid = userListEdu.ElementAt(eid);

                if (UID == eduid)
                {

                    var data = (from user in _userContext.Users
                                where user.UID == UID
                                join cv in _userContext.CV_s on user.UID equals cv.UID into cvGroup
                                from cv in cvGroup.DefaultIfEmpty()

                                join cvEdu in _userContext.CV_Educations on cv.CID equals cvEdu.CID into cvEduGroup
                                from cvEdu in cvEduGroup.DefaultIfEmpty()

                                join edu in _userContext.Education on cvEdu.EdID equals edu.EdID into eduGroup
                                from edu in eduGroup.DefaultIfEmpty()

                                select new { user.UID, user.Firstname, user.Lastname, user.Username, user.Privat, Description = (edu == null ? null : edu.Description) }).ToList();

                    ViewBag.Education = data;
                }
                else
                {
                    ViewBag.EducationMessage = "Saknar Utbildning";
                }
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

                    var competence = (from user in _userContext.CV_s
                                      where user.UID == UID
                                      join cv in _userContext.CV_s on user.UID equals cv.UID into cvGroup
                                      from cv in cvGroup.DefaultIfEmpty()

                                      join comp in _userContext.CV_Competences on cv.CID equals comp.CompID into cvCompGroup
                                      from comp in cvCompGroup.DefaultIfEmpty()

                                      join cvC in _userContext.Competence on cv.CID equals cvC.CompID into CompGroup
                                      from cvC in CompGroup.DefaultIfEmpty()

                                      select new { cvC.Description }).ToList();

                    ViewBag.Competence = competence;
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
                        //funkar
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

                var userListExp = (from id in _userContext.Experience
                                   select id.CID).ToList();


                for (int expid = 0; expid < userListEdu.Count(); expid++)
                {

                    int expIdList = userListExp.ElementAt(expid);

                    if (UID == expIdList)
                    {
                        var experience = (from user in _userContext.Users
                                          where user.UID == UID
                                          join cv in _userContext.CV_s on user.UID equals cv.UID into cvUser
                                          from cv in cvUser.DefaultIfEmpty()

                                          join exp in _userContext.Experience on cv.CID equals exp.CID into expCV
                                          from exp in expCV.DefaultIfEmpty()
                                          select new { exp.Description });

                        ViewBag.Experience = experience;


                    }
                    else
                    {
                        ViewBag.ExperienceMessage = "Saknar Erfarenheter";
                    }
                }
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
        public async Task <IActionResult> AddEducation(Education newEducation)
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

            List<int> ComptenceID  = _userContext.CV_Competences.Where(x => x.CID.Equals(CVID)).Select(x => x.CompID).ToList();
            List<Competence> Competences = new List<Competence>();
            foreach (int ID in ComptenceID)
            {
                Competence oneCompetence = _userContext.Competence.Where(x => x.CompID.Equals(ID)).FirstOrDefault();
                if(oneCompetence != null) 
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

