using Laja.Models;
using Laja.Services;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace Laja.Controllers
{
    [Authorize]
    public class ActivitiesController : Controller
    {

        private readonly ApplicationDbContext db;
        private ValidationService validationService;


        public ActivitiesController()
        {
            db = new ApplicationDbContext();
            validationService = new ValidationService(db);
        }

        // GET: Activities
        public ActionResult Index()
        {
            var activities = db.Activities.Include(a => a.ActivityType).Include(a => a.Module);
            return View(activities.ToList());

           
        }

        // GET: Activities/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Activity activity = db.Activities.Find(id);
            if (activity == null)
            {
                return HttpNotFound();
            }

            return View(activity);
        }

        // GET: Activities/Create
        [Authorize(Roles = "Lärare")]
        public ActionResult Create(int? moduleId)
        {
            ViewBag.ActivityTypeId = new SelectList(db.ActivityTypes, "Id", "Name");
            //ViewBag.ModuleId = new SelectList(db.Modules, "Id", "Name");



            if (moduleId != null)
            {
                ViewBag.ModuleId = moduleId;
                var module = db.Modules.Find(moduleId);
                ViewBag.ModuleName = module.Name + " (" + module.StartDate.ToShortDateString() + " - " + module.EndDate.ToShortDateString() + " )";
                ViewBag.ModuleStart = module.StartDate.ToShortDateString();
                ViewBag.ModuleEnd = module.EndDate.ToShortDateString();
                //var moduleTempId = db.Activities.Find()
                var courseId = db.Modules.Find(moduleId).CourseId;
                ViewBag.CourseId = courseId;

            }
            return View();
        }

        // POST: Activities/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Lärare")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,ModuleId,Description,StartDate,EndDate,DeadLine,SubmitRequired,ActivityTypeId")] Activity activity)
        {
            if (ModelState.IsValid)
            {
                var activityExists = validationService.UniqName(activity);
                if (activityExists)
                {
                    ViewBag.Error = "Aktitivtetnamnet används redan. Var god ange ett annat namn, tack.";
                    ViewBag.ActivityTypeId = new SelectList(db.ActivityTypes, "Id", "Name", activity.ActivityTypeId);
                    return View(activity);
                }
                if (!validationService.CheckActivityPeriodAgainstModule(activity))
                {
                    ViewBag.Error = "Aktivitetens startdatum och slutdatum måste vara inom moduless start och slutdatum.";
                    ViewBag.ActivityTypeId = new SelectList(db.ActivityTypes, "Id", "Name", activity.ActivityTypeId);
                    return View(activity);
                }
                if (activity.DeadLine != null)
                    activity.SubmitRequired = true;

                db.Activities.Add(activity);
                db.SaveChanges();

                return RedirectToAction("Index", "Teacher", new { @CourseId = db.Modules.Find(activity.ModuleId).CourseId });
            }

            ViewBag.ActivityTypeId = new SelectList(db.ActivityTypes, "Id", "Name", activity.ActivityTypeId);
            ViewBag.ModuleId = new SelectList(db.Modules, "Id", "Name", activity.ModuleId);
            return View(activity);
        }

        // GET: Activities/Edit/5
        [Authorize(Roles = "Lärare")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Activity activity = db.Activities.Find(id);
            if (activity == null)
            {
                return HttpNotFound();
            }
            ViewBag.ActivityTypeId = new SelectList(db.ActivityTypes, "Id", "Name", activity.ActivityTypeId);
            ViewBag.ModuleId = activity.ModuleId;
            var module = db.Modules.Find(activity.ModuleId);
            ViewBag.ModuleStart = module.StartDate.ToShortDateString();
            ViewBag.ModuleEnd = module.EndDate.ToShortDateString();
            //ViewBag.ModuleId = new SelectList(db.Modules, "Id", "Name", activity.ModuleId);
            ViewBag.CourseId = module.CourseId;
            return View(activity);
        }

        // POST: Activities/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Lärare")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,ModuleId,Description,StartDate,EndDate,DeadLine,SubmitRequired,ActivityTypeId")] Activity activity)
        {
            if (ModelState.IsValid)
            {
                db.Entry(activity).State = EntityState.Modified;
                db.SaveChanges();
                //return RedirectToAction("Index");
                return RedirectToAction("Index", "Teacher", new { @CourseId = db.Modules.Find(activity.ModuleId).CourseId });
            }
            ViewBag.ActivityTypeId = new SelectList(db.ActivityTypes, "Id", "Name", activity.ActivityTypeId);
            ViewBag.ModuleId = activity.ModuleId;
            return View(activity);
        }

        // GET: Activities/Delete/5
        [Authorize(Roles = "Lärare")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Activity activity = db.Activities.Find(id);
            if (activity == null)
            {
                return HttpNotFound();
            }
            return View(activity);
        }

        // POST: Activities/Delete/5
        [Authorize(Roles = "Lärare")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Activity activity = db.Activities.Find(id);
            db.Activities.Remove(activity);
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
