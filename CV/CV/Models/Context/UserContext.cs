using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace CV.Models.Context
{
    public class UserContext : DbContext
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



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
             base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User_Project>().HasKey(u => new { u.PID, u.UID });

            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<CV_Competence>().HasKey(u => new { u.CID, u.COID });

            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<CV_Education>().HasKey(u => new { u.CID, u.EID });
        }
    }
}

