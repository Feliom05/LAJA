using Laja.Models;
using Laja.Services;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace Laja.Controllers
{
    [Authorize(Roles = "Lärare")]
    public class TeacherController : Controller
    {
        private readonly ApplicationDbContext db;
        //private ValidationService validationService;


        public TeacherController()
        {
            db = new ApplicationDbContext();
           // validationService = new ValidationService(db);
        }
        // GET: Teacher
        public ActionResult Index(int? courseId)    
        {
            if (courseId != null)
            {
                var course = db.Courses.Where(c => c.Id == courseId).Include(c => c.Documents).Include(c => c.Students).Include(c => c.Modules).FirstOrDefault();
                return View(course);
            }
            else
            {
                return RedirectToAction("Index", "Courses");
            }
            return View();
        }
    }
}