using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Identity;
using CV.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Models
{
    public class UserContext : IdentityDbContext<LogInUser>
    {
        public UserContext(DbContextOptions<UserContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Chat> Chats { get; set; }

        public DbSet<CV_> CV_s { get; set; }

        public DbSet<Project> Projects { get; set; }


        public DbSet<Education> Education { get; set; }
        public DbSet<Experience> Experience { get; set; }

        public DbSet<Competence> Competence { get; set; }

        public DbSet<User_Project> UserProjects { get; set; }

        public DbSet<CV_Competence> CV_Competences { get; set; }

        public DbSet<CV_Education> CV_Educations { get; set; }

        //Ska tas bort sen, används för test data 
        public virtual IEnumerable<Experience> Ex { get; set; } = new List<Experience>();

        public virtual IEnumerable<Education> Ed { get; set; } = new List<Education>();

        public virtual IEnumerable<Competence> Co { get; set; } = new List<Competence>();



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User_Project>().HasKey(u => new { u.PID, u.UID });

            modelBuilder.Entity<CV_Competence>().HasKey(u => new { u.CID, u.CompID });

            modelBuilder.Entity<CV_Education>().HasKey(u => new { u.CID, u.EdID });

            //Exempeldata

            #region Project

            //In this code, DeleteBehavior.Restrict disables the cascade delete. Now, when a User or Project is deleted, the related User_Project will not be deleted automatically,
            //preventing the multiple cascade paths issue. You’ll need to handle the deletion of User_Project manually in your code when a User or Project is deleted.


            modelBuilder.Entity<User_Project>()
                .HasOne(up => up.user)
                .WithMany(u => u.User_Projects)
                .HasForeignKey(up => up.UID)
                .OnDelete(DeleteBehavior.Restrict); 

            modelBuilder.Entity<User_Project>()
                .HasOne(up => up.project)
                .WithMany(p => p.User_Projects)
                .HasForeignKey(up => up.PID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Project>().HasData(
            new Project
            {
                PID = 1,
                Title = "Web Development Project",
                Description = "Developing a responsive web application.",
                BeginDate = new DateTime(2022, 1, 1),
                EndDate = new DateTime(2022, 6, 30),
                UserCreatedBy = 1
            },
                new Project
                {
                    PID = 2,
                    Title = "Mobile App Development",
                    Description = "Creating a cross-platform mobile application.",
                    BeginDate = new DateTime(2022, 3, 15),
                    EndDate = new DateTime(2022, 9, 15),
                    UserCreatedBy = 2
                },
                new Project
                {
                    PID = 3,
                    Title = "Database Management System",
                    Description = "Designing and implementing a relational database.",
                    BeginDate = new DateTime(2022, 5, 1),
                    EndDate = new DateTime(2022, 11, 30),
                    UserCreatedBy = 3
                },
                new Project
                {
                    PID = 4,
                    Title = "Machine Learning Project",
                    Description = "Applying machine learning algorithms to solve a specific problem.",
                    BeginDate = new DateTime(2022, 7, 1),
                    EndDate = new DateTime(2023, 1, 31),
                    UserCreatedBy = 4
                },
                new Project
                {
                    PID = 5,
                    Title = "Agile Software Development",
                    Description = "Working on a software project using agile methodologies.",
                    BeginDate = new DateTime(2022, 9, 15),
                    EndDate = new DateTime(2023, 3, 15),
                    UserCreatedBy = 5
                }
            );
            #endregion
            #region CV_
            modelBuilder.Entity<CV_>().HasData(
            new CV_
            {
                CID = 1,
                Experiences = Ex,
                Education = Ed,
                Competence = Co,
                Picture = "profile_picture1.jpg",
                UID = 1
            },
                new CV_
                {
                    CID = 2,
                    Experiences = Ex,
                    Education = Ed,
                    Competence = Co,
                    Picture = "profile_picture2.jpg",
                    UID = 2
                },
                new CV_
                {
                    CID = 3,
                    Experiences = Ex,
                    Education = Ed,
                    Competence = Co,
                    Picture = "profile_picture3.jpg",
                    UID = 3
                },
                new CV_
                {
                    CID = 4,
                    Experiences = Ex,
                    Education = Ed,
                    Competence = Co,
                    Picture = "profile_picture4.jpg",
                    UID = 4
                },
                new CV_
                {
                    CID = 5,
                    Experiences = Ex,
                    Education = Ed,
                    Competence = Co,
                    Picture = "profile_picture5.jpg",
                    UID = 5
                }
               );
            #endregion
            #region Competence
            modelBuilder.Entity<Competence>().HasData(
                 new Competence
                 {
                     CompID = 1,
                     Description = "C# Programming"
                 },
                    new Competence
                    {
                        CompID = 2,
                        Description = "Web Development"
                    },
                    new Competence
                    {
                        CompID = 3,
                        Description = "Data Analysis"
                    },
                    new Competence
                    {
                        CompID = 4,
                        Description = "Project Management"
                    },
                    new Competence
                    {
                        CompID = 5,
                        Description = "Communication Skills"
                    }
                    );
            #endregion
            #region Experience
            modelBuilder.Entity<Experience>().HasData(
             new Experience
             {
                 EID = 1,
                 Description = "Software Developer at ABC Tech",
                 CID = 1 // CV-ID
             },
            new Experience
            {
                EID = 2,
                Description = "Data Analyst at XYZ Analytics",
                CID = 2 // CV-ID
            },
            new Experience
            {
                EID = 3,
                Description = "Project Manager at Acme Projects",
                CID = 3 // CV-ID
            },
            new Experience
            {
                EID = 4,
                Description = "Internship at DEF Corporation",
                CID = 4 // CV-ID
            },
            new Experience
            {
                EID = 5,
                Description = "Marketing Coordinator at LMN Marketing",
                CID = 5 // CV-ID
            }
            );
            #endregion
            #region Education
            modelBuilder.Entity<Education>().HasData(
                    new Education { EdID = 1, Description = "Master's in Computer Science", BeginDate = new DateTime(2020, 1, 1), EndDate = new DateTime(2023, 1, 1) },
                    new Education { EdID = 2, Description = "Bachelor's in Information Technology", BeginDate = new DateTime(2020, 1, 1), EndDate = new DateTime(2023, 1, 1) },
                    new Education { EdID = 3, Description = "High School Diploma", BeginDate = new DateTime(2020, 1, 1), EndDate = new DateTime(2023, 1, 1) },
                    new Education { EdID = 4, Description = "Certification in Web Development", BeginDate = new DateTime(2020, 1, 1), EndDate = new DateTime(2023, 1, 1) },
                    new Education { EdID = 5, Description = "Online Course in Data Science", BeginDate = new DateTime(2020, 1, 1), EndDate = new DateTime(2023, 1, 1) }
                );

            #endregion
            #region User
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    UID = 1,
                    Username = "user1",
                    Firstname = "Alice",
                    Lastname = "Johnson",
                    Password = "password1",
                    ConfirmPassword = "password1",
                    Epost = "alice.johnson@example.com",
                    Adress = "456 Oak Street",
                    Privat = true

                },
                new User
                {
                    UID = 2,
                    Username = "user2",
                    Firstname = "Bob",
                    Lastname = "Smith",
                    Password = "password2",
                    ConfirmPassword = "password2",
                    Epost = "bob.smith@example.com",
                    Adress = "789 Pine Avenue",
                    Privat = false
                },
                new User
                {
                    UID = 3,
                    Username = "user3",
                    Firstname = "Charlie",
                    Lastname = "Brown",
                    Password = "password3",
                    ConfirmPassword = "password3",
                    Epost = "charlie.brown@example.com",
                    Adress = "101 Elm Lane",
                    Privat = true
                },
                new User
                {
                    UID = 4,
                    Username = "user4",
                    Firstname = "David",
                    Lastname = "Lee",
                    Password = "password4",
                    ConfirmPassword = "password4",
                    Epost = "david.lee@example.com",
                    Adress = "202 Maple Road",
                    Privat = false
                },
                new User
                {
                    UID = 5,
                    Username = "user5",
                    Firstname = "Eva",
                    Lastname = "Miller",
                    Password = "password5",
                    ConfirmPassword = "password5",
                    Epost = "eva.miller@example.com",
                    Adress = "303 Cedar Street",
                    Privat = true
                }
                );
            #endregion
            #region Chat
            //Tar inte bort meddelande om en användare raderas.
            modelBuilder.Entity<Chat>()
                        .HasOne(m => m.Sender)
                        .WithMany(u => u.ChatsSent)
                        .HasForeignKey(m => m.SenderID)
                        .OnDelete(DeleteBehavior.NoAction); 


            modelBuilder.Entity<Chat>()
                        .HasOne(m => m.Receiver)
                        .WithMany(u => u.ChatsReceived)
                        .HasForeignKey(m => m.ReceiverID)
                        .OnDelete(DeleteBehavior.NoAction); 


            modelBuilder.Entity<Chat>().HasData(
                 new Chat
                 {
                     MID = 1,
                     Text = "Hej, hur mår du?",
                     Date = new DateTime(2022, 1, 10, 12, 30, 0),
                     Read = true,
                     SenderID = 1, // Användarens ID
                     ReceiverID = 2
                 },
                new Chat
                {
                    MID = 2,
                    Text = "Jag mår bra, tack!",
                    Date = new DateTime(2022, 1, 10, 12, 35, 0),
                    Read = true,
                    SenderID = 2,
                    ReceiverID = 1
                    // Användarens ID
                },
                new Chat
                {
                    MID = 3,
                    Text = "Vad har du gjort idag?",
                    Date = new DateTime(2022, 1, 10, 13, 0, 0),
                    Read = false,
                    SenderID = 3,
                    ReceiverID = 4
                    // Användarens ID
                },
                new Chat
                {
                    MID = 4,
                    Text = "Jobbat och tränat lite.",
                    Date = new DateTime(2022, 1, 10, 13, 15, 0),
                    Read = false,
                    SenderID = 4,
                    ReceiverID = 3
                    // Användarens ID
                },
                new Chat
                {
                    MID = 5,
                    Text = "Låter bra! Vad har du för planer resten av dagen?",
                    Date = new DateTime(2022, 1, 10, 14, 0, 0),
                    Read = false,
                    SenderID = 1,
                    ReceiverID = 2
                    // Användarens ID
                }
                );
            #endregion
            #region CV_Competence
            modelBuilder.Entity<CV_Competence>().HasData(
            new CV_Competence
                {
                    CID = 1,
                    CompID = 2

                },
             new CV_Competence
             {
                 CID = 2,
                 CompID = 3

             }, new CV_Competence
             {
                 CID = 4,
                 CompID = 5

             }, new CV_Competence
             {
                 CID = 1,
                 CompID = 4

             }, new CV_Competence
             {
                 CID = 4,
                 CompID = 2

             }
            );
            #endregion
            #region CV_Education
            modelBuilder.Entity<CV_Education>().HasData(
            new CV_Education
            {
                CID = 1,
                EdID = 2,

            },
             new CV_Education
             {
                 CID = 2,                
                 EdID = 3,

             }, new CV_Education
             {
                 CID = 4,
                 EdID = 5,

             }, new CV_Education
             {
                 CID = 1,
                 EdID = 4,

             }, new CV_Education
             {
                 CID = 4,
                 EdID = 2,
                 
             }
            );
            #endregion
            #region User_Project
            modelBuilder.Entity<User_Project>().HasData(
            new User_Project
            {                
                UID = 1,
                PID = 2
            }, 
             new User_Project
             {
                 UID = 2,
                 PID = 3

             }, new User_Project
             {
                 UID = 4,
                 PID = 5

             }, new User_Project
             {
                 UID = 1,
                 PID = 4

             }, new User_Project
             {
                 UID = 4,
                 PID = 2
             }
            );
            #endregion
        }
    }
}

