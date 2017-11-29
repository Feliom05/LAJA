using Laja.Models;
using Laja.Services;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace Laja.Controllers

{
    [Authorize]
    public class CoursesController : Controller
    {
        private readonly ApplicationDbContext db;
        private ValidationService validationService;

        public CoursesController()
        {
            db = new ApplicationDbContext();
            validationService = new ValidationService(db);
        }

        // GET: Courses
        public ActionResult Index()
        {
            return View(db.Courses.ToList());
        }

        // GET: Courses/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        // GET: Courses/Create
        [Authorize(Roles ="Lärare")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Courses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Lärare")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Description,StartDate,EndDate")] Course course)
        {
            if (ModelState.IsValid)
            {
               
                var NameExists = validationService.UniqName(course);
                if (NameExists)
                {
                    ViewBag.Error = "Kursnamnet används redan. Var god ange ett annat namn, tack.";
                    return View(course);
                }
                if (!validationService.CheckPeriod(course))
                {
                    ViewBag.Error = "Slut datum måste vara efter startdatum.";
                    return View(course);
                }

                db.Courses.Add(course);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(course);
        }

        // GET: Courses/Edit/5
        [Authorize(Roles = "Lärare")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        // POST: Courses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Lärare")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Description,StartDate,EndDate")] Course course)
        {
            if (ModelState.IsValid)
            {

               
                var NameExists = validationService.UniqName(course);
                if (NameExists)
                {
                    ViewBag.Error = "Kursnamnet används redan. Var god ange ett annat namn, tack.";
                    return View(course);
                }
                if (!validationService.CheckPeriod(course))
                {
                    ViewBag.Error = "Slut datum måste vara efter startdatum.";
                    return View(course);
                }
                if (!validationService.CheckCoursePeriodAgainstModules(course))
                {
                    ViewBag.Error = "Kursens start eller slutdatum får inte vara före eller efter den första och sista modulen.";
                    return View(course);
                }


                db.Entry(course).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(course);
        }

        // GET: Courses/Delete/5
        [Authorize(Roles = "Lärare")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        // POST: Courses/Delete/5
        [Authorize(Roles = "Lärare")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Course course = db.Courses.Find(id);
            db.Courses.Remove(course);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
