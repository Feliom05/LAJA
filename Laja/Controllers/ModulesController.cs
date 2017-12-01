using Laja.Models;
using Laja.Services;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace Laja.Controllers
{
    [Authorize]
    public class ModulesController : Controller
    {
        private readonly ApplicationDbContext db;
        private ValidationService validationService;

        public ModulesController()
        {
            db = new ApplicationDbContext();
            validationService = new ValidationService(db);
        }

        // GET: Modules
        public ActionResult Index()
        {
            var modules = db.Modules.Include(m => m.Course);
            return View(modules.ToList());
        }

        // GET: Modules/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Module module = db.Modules.Find(id);
            if (module == null)
            {
                return HttpNotFound();
            }
            return View(module);
        }

        // GET: Modules/Create
        [Authorize(Roles = "Lärare")]
        public ActionResult Create(int? courseId)
        {
            //ViewBag.CourseId = new SelectList(db.Courses, "Id", "Name");
            if (courseId != 0)
            {
                ViewBag.CourseSelectedId = courseId;
                var course = db.Courses.Find(courseId);
                var name = course.Name;
                ViewBag.CourseName = name;
                
                ViewBag.CourseStart = course.StartDate.ToShortDateString();
                ViewBag.CourseEnd = course.EndDate.ToShortDateString();

            }


            return View();
        }

        // POST: Modules/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Lärare")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,CourseId,Description,StartDate,EndDate")] Module module)
        {
            if (ModelState.IsValid)
            {

                var moduleExists = validationService.UniqName(module);
                if (moduleExists)
                {
                    ViewBag.Error = "Modulnamnet används redan. Var god ange ett annat namn, tack.";
                    return View(module);
                }
                if (!validationService.CheckModulePeriodAgainstCourse(module))
                {
                    ViewBag.Error = "Modulens startdatum och slutdatum måste vara inom kursens start och slutdatum.";
                    return View(module);
                }


                db.Modules.Add(module);
                db.SaveChanges();
                return RedirectToAction("Details", "Courses", new { @id = module.CourseId });
                //}
                //else
                //{
                //    ViewBag.CourseId = new SelectList(db.Courses, "Id", "Name", module.CourseId);
                //    ViewBag.Error = "Modulenamnet används redan inom samma kurs. Var god ange ett annat namn, tack.";
                //    return View(module);
                //}


            }

            ViewBag.CourseId = new SelectList(db.Courses, "Id", "Name", module.CourseId);
            return View(module);
        }

        // GET: Modules/Edit/5
        [Authorize(Roles = "Lärare")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Module module = db.Modules.Find(id);
            if (module == null)
            {
                return HttpNotFound();
            }
            ViewBag.CourseId = new SelectList(db.Courses, "Id", "Name", module.CourseId);
            return View(module);
        }

        // POST: Modules/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Lärare")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,CourseId,Description,StartDate,EndDate")] Module module)
        {
            if (ModelState.IsValid)
            {
                db.Entry(module).State = EntityState.Modified;
                db.SaveChanges();
               // return RedirectToAction("Index");
                return RedirectToAction("Details", "Courses", new { id = module.CourseId });
            }
            ViewBag.CourseId = new SelectList(db.Courses, "Id", "Name", module.CourseId);
            return View(module);
        }

        // GET: Modules/Delete/5
        [Authorize(Roles = "Lärare")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Module module = db.Modules.Find(id);
            if (module == null)
            {
                return HttpNotFound();
            }
            return View(module);
        }

        // POST: Modules/Delete/5
        [Authorize(Roles = "Lärare")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Module module = db.Modules.Find(id);
            db.Modules.Remove(module);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult ModuleWithActivities(int id = 0)
        {
            var module = db.Modules.Find(id);
            return PartialView("_ModulepartialView", module);
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
