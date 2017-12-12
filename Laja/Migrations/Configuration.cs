namespace Laja.Migrations
{
    using Laja.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Laja.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(ApplicationDbContext context)
        {
            AddRoles(context);
            AddUsers(context);
            AddTeacherRole(context);

            var course = new Course
            {
                Name = "ASP .NET",
                Description = "Lär dig .NET på 21 dagar",
                StartDate = new DateTime(2017, 08, 30),
                EndDate = new DateTime(2017, 12, 15)
            };
            context.Courses.AddOrUpdate(k => k.Id,
                course);

            context.SaveChanges();

            var modul = new Module
            {
                Name = "Slutprojekt",
                Description = "Laja Education",
                StartDate = new DateTime(2017, 12, 01),
                EndDate = new DateTime(2017, 12, 15),
                CourseId = course.Id
            };

            context.Modules.AddOrUpdate(m => m.Id,
                modul);

            context.SaveChanges();

            var actType = new ActivityType
            {
                Name = "Inlämningsuppgift"
            };

            context.ActivityTypes.AddOrUpdate(a => a.Id, actType);

            context.SaveChanges();

            var act = new Activity
            {
                Name = "Inlämningsuppgift",
                Description = "inlupp 1.0",
                StartDate = new DateTime(2017, 12, 02),
                EndDate = new DateTime(2017, 12, 04),
                DeadLine = new DateTime(2017, 12, 05),
                ModuleId = modul.Id,
                ActivityTypeId = actType.Id

            };

            context.Activities.AddOrUpdate(a => a.Id, act);

            context.SaveChanges();


            //var modul = AddModules(context);
            //AddCourse(context, modul);
        }

        //private void AddCourse(ApplicationDbContext context, List<Module> module)
        //{
        //    var kurs = new List<Course>()
        //    {
        //        new Course { Id = 1, Description = "ASP .NET", StartDate = new DateTime(2017, 08, 30),
        //            EndDate = new DateTime(2017, 08, 30)}
        //    };
        //}

        //private List<Module> AddModules(ApplicationDbContext context)
        //{
        //    var modul = new List<Module>()
        //    {
        //        new Module { Id = 1, Name = "ASP .NET", StartDate = new DateTime(2017, 08, 30),
        //            EndDate = new DateTime(2017, 08, 30)}
        //    };

        //    return modul;
        //}

        private void AddRoles(ApplicationDbContext context)
        {
            var roleStore = new RoleStore<IdentityRole>(context);
            var roleManager = new RoleManager<IdentityRole>(roleStore);
            var roleNames = new[] { "Lärare", "Elev" };
            foreach (var roleName in roleNames)
            {
                if (context.Roles.Any(r => r.Name == roleName)) continue;
                var role = new IdentityRole { Name = roleName };
                var result = roleManager.Create(role);
                if (!result.Succeeded)
                {
                    throw new Exception(string.Join("\n", result.Errors));
                }

            }
        }

        private void AddUsers(ApplicationDbContext context)
        {
            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);
            var AppUsers = new List<ApplicationUser>()
            {
                new ApplicationUser {UserName = "larare1@laja.se", Email = "larare1@laja.se", FirstName="Kalle", LastName = "Karlsson" },
                new ApplicationUser {UserName = "larare2@laja.se", Email = "larare2@laja.se", FirstName="Dimitris", LastName = "Björlingh" },
                new ApplicationUser {UserName = "elev@laja.se", Email = "elev@laja.se", FirstName="elev1", LastName = "Elev1" },

            };
            foreach (var appUser in AppUsers)
            {
                var user = context.Users.Where(u => u.Email == appUser.Email).FirstOrDefault();
                if (user != null && user.Id != "")
                {
                    user.FirstName = appUser.FirstName;
                    user.LastName = appUser.LastName;
                    userManager.Update(user);
                }
                else
                {
                    userManager.Create(appUser, "Abc_123");
                }
            }
        }

        private void AddTeacherRole(ApplicationDbContext context)
        {
            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);
            var adminRole = userManager.FindByName("larare2@laja.se");
            if (adminRole != null)
            {
                var result = userManager.AddToRole(adminRole.Id, "Lärare");
            }
            var elevRole = userManager.FindByName("elev@laja.se");
            if (adminRole != null)
            {
                var result = userManager.AddToRole(adminRole.Id, "Elev");
            }
        }
    }
}
