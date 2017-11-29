namespace Laja.Migrations
{
    using Laja.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
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

        }
        private void AddRoles(ApplicationDbContext context)
        {
            var roleStore = new RoleStore<IdentityRole>(context);
            var roleManager = new RoleManager<IdentityRole>(roleStore);
            var roleNames = new[] { "L�rare", "Elev" };
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
                new ApplicationUser {UserName = "larare2@laja.se", Email = "larare2@laja.se", FirstName="Dimitris", LastName = "Bj�rlingh" },
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
                var result = userManager.AddToRole(adminRole.Id, "L�rare");
            }
            var elevRole = userManager.FindByName("elev@laja.se");
            if (adminRole != null)
            {
                var result = userManager.AddToRole(adminRole.Id, "Elev");
            }
        }
    }
}
