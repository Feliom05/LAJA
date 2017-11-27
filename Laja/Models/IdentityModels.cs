using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System.EnterpriseServices;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Laja.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        //nav
        public Course Course { get; set; }

    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<Course> Courses { get; set; }

        public DbSet<Module> Modules { get; set; }

        public DbSet<Activity> Activities { get; set; }

        public DbSet<ActivityType> ActivityTypes { get; set; }

        public DbSet<Document> Documents { get; set; }

        public DbSet<DocType> DocTypes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Course>()
                .HasMany<Module>(m => m.Modules)
                .WithRequired(m => m.Course)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Module>()
                .HasMany<Activity>(a => a.Activities)
                .WithRequired(a => a.Module)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ActivityType>()
                .HasMany<Activity>(a => a.Activities)
                .WithRequired(a => a.ActivityType)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<DocType>()
                .HasMany<Document>(t => t.Documents)
                .WithRequired(t => t.DocType)
                .WillCascadeOnDelete(false);

            base.OnModelCreating(modelBuilder);
        }
       
    }
}