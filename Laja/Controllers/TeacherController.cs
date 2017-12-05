using Laja.Models;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

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
                var course = db.Courses
                    .Where(c => c.Id == courseId)
                    .Include(c => c.Documents)
                    .Include(c => c.Students)
                    .Include(c => c.Modules)
                    .FirstOrDefault();
                course.Modules = course.Modules.OrderBy(m => m.StartDate).ToList();

                return View(course);
            }
            else
            {
                return RedirectToAction("Index", "Courses");
            }
        }
    }
}