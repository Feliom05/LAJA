using Laja.Models;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
namespace Laja.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            if (User.IsInRole("Lärare"))
            {
                return RedirectToAction("Index", "Courses");
            }
            else if (User.IsInRole("Elev"))
            {
                return RedirectToAction("Index", "Student");
            }
            else
            {
                ViewBag.Courses = db.Courses.Include(c => c.Modules).Include(c => c.Students).ToList();
                return View();
            }
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }
    }
}