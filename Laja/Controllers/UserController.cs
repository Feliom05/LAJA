using Laja.Models;
using Laja.Services;
using Laja.ViewModels;
using System.Web.Mvc;

namespace Laja.Controllers
{
    public class UserController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: User
        public ActionResult Create()
        {
            var user = new UserViewModel();
            return View(user);
        }

        // POST: Courses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "FirstName, LastName, Email, CourseId")] UserViewModel User)
        {
            if (ModelState.IsValid)
            {
                var validateEmail = new ValidationService(db);
            }

            return View();
        }
    }
}