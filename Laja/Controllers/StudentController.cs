using Laja.Models;
using Laja.Services;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace Laja.Controllers
{
    [Authorize]
    public class StudentController : Controller
    {
        private readonly ApplicationDbContext db;
        private ValidationService validationService;


        public StudentController()
        {
            db = new ApplicationDbContext();
            validationService = new ValidationService(db);
        }
        // GET: Student

        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var user = db.Users.Where(u => u.Id == userId).FirstOrDefault();
            if (User.IsInRole("Elev"))
            {
                var course = db.Courses.Where(c => c.Id == user.CourseId).FirstOrDefault();
                return  View(course);
            }
            else
            {
                // to comming teacher page
            }
            return View();
        }
    }
}