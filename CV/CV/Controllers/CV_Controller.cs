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

      /*public IActionResult Index(int UID)
        {


            var currentUser = (from u in _userContext.Users
                               where u.Firstname == User.Identity.Name
                               select u.UID);
            UID = currentUser.First();

            AddResume(UID);
			return RedirectToAction("AddResume", "CV_");

			
		}*/

		/*		-----------------------------------------------------*/


		[HttpGet]
        public IActionResult Edit(int id)
        {
            ViewBag.Username = _userContext.Users.Select(x => new SelectListItem { Text = x.Firstname, Value = x.UID.ToString() });
            CV_ cv = _userContext.CV_s.FirstOrDefault(x => x.UID.Equals(id));


            return View(cv);

        }

        [HttpPost]
        public IActionResult Edit(CV_ cv)
        {
            _userContext.CV_s.Update(cv);
            _userContext.SaveChanges();
            return RedirectToAction("AddResume", "CV_");
        }


        public IActionResult AddResume(int UID)
        {
            if(UID == 0)
            {
				var currentUser = (from u in _userContext.Users
								   where u.Firstname == User.Identity.Name
								   select u.UID);
				UID = currentUser.First();
			}

           
            /*            ProfilBild
			*/
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




                    string filePath = "C:\\Users\\Admin\\OneDrive\\Dokument\\Webbsystem(.NET)\\CVProjekt\\CV\\CV\\wwwroot\\Pictures\\" + profilePicture + "";


                    string FilePathExists = Path.Combine(filePath);

                    if (!System.IO.File.Exists(FilePathExists))
                    {
                        ViewBag.noProfilePicture = "no_profile_picture.jpg";
                    }




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
                if(fullnameElement.Equals(null))
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

                    if (UID == expid)
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


    }







}

