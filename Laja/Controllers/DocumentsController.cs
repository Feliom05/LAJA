using Laja.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Laja.Controllers
{
    [Authorize]
    public class DocumentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Documents
        public ActionResult Index()
        {
            var documents = db.Documents.Include(d => d.Activity).Include(d => d.Course).Include(d => d.DocType).Include(d => d.Module).Include(d => d.User);
            return View(documents.ToList());
        }


        // GET: Documents/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Document document = db.Documents.Find(id);
            if (document == null)
            {
                return HttpNotFound();
            }
            return View(document);
        }

        // GET: Documents/Create
        public ActionResult Create(int? id, string c)
        {

            TempData.Remove("c");
            TempData.Add("c", c);

            //Candidate for refactoring - To find the Course ID to be used by "Back To" in the Create View

            int courseIdToBeUsedForBack = 0;
            switch (c)
            {
                case "course":
                    {
                        var course = db.Courses.Find(id);
                        courseIdToBeUsedForBack = course.Id;
                        break;
                    }
                case "module":
                    {
                        courseIdToBeUsedForBack = db.Modules.Find(id).CourseId;
                        break;
                    }
                case "activity":
                    {
                        var module = db.Activities.Find(id);
                        var course = db.Modules.Find(module.ModuleId);
                        courseIdToBeUsedForBack = course.CourseId;
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
            ViewBag.CourseId = courseIdToBeUsedForBack;

            return View();
        }

        // POST: Documents/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Document document, HttpPostedFileBase upload1, int? id, string c)
        {
            ViewBag.Message = "";
            foreach (string upload in Request.Files)
            {
                if (Request.Files[upload].FileName != "")
                {
                    string path = AppDomain.CurrentDomain.BaseDirectory + "/App_Data/Documents/";
                    string filename = Path.GetFileName(Request.Files[upload].FileName);
                    string ext = filename.Split('.')[1];
                    if (db.DocTypes.Where(e => e.Extension.Equals(ext)).FirstOrDefault() != null)
                    {
                        c = TempData["c"].ToString();
                        switch (c)
                        {
                            case "course":
                                document.CourseId = id;
                                break;
                            case "module":
                                document.ModuleId = id;
                                break;
                            case "activity":
                                document.ActivityId = id;
                                break;
                            default:
                                break;
                        }
                        var currentUser = User.Identity.GetUserName();
                        document.UserId = User.Identity.GetUserId().ToString();
                        document.CreationTime = DateTime.Now;
                        filename = filename.Replace("_", "-");
                        filename = filename + "_" + DateTime.Now.ToString();
                        filename = filename.Replace(":", "_");
                        document.FileName = filename;
                        document.DocTypeId = db.DocTypes.Where(f => f.Extension == ext).FirstOrDefault().Id;
                        var folder = Directory.CreateDirectory(Path.Combine(path, currentUser)).FullName;
                        Request.Files[upload].SaveAs(Path.Combine(folder, filename));
                        var filePath = Path.Combine(folder, filename);
                        String RelativePath = filePath.Replace(Request.ServerVariables["APPL_PHYSICAL_PATH"], String.Empty);
                        document.Name = RelativePath;
                        db.Documents.Add(document);
                        db.SaveChanges();
                    }
                    else
                    {
                        ViewBag.Message = "File type not allowed, please try another file!";
                    }
                }
            }
            if (ModelState.IsValid)
            {

                return RedirectToAction("Index");
            }

            ViewBag.ActivityId = new SelectList(db.Activities, "Id", "Name", document.ActivityId);
            ViewBag.CourseId = new SelectList(db.Courses, "Id", "Name", document.CourseId);
            ViewBag.DocTypeId = new SelectList(db.DocTypes, "Id", "Extension", document.DocTypeId);
            ViewBag.ModuleId = new SelectList(db.Modules, "Id", "Name", document.ModuleId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName", document.UserId);


            // Candidate to be refactored
            int? backToId;
            if (document.CourseId != null)
            {
                backToId = document.CourseId;
            }
            else if (document.ModuleId != null)
            {
                var course = db.Modules.Find(document.ModuleId);
                backToId = course.CourseId;
            }
            else
            {
                var module = db.Activities.Find(document.ActivityId);
                var course = db.Modules.Find(module.ModuleId);
                backToId = course.CourseId;
            }

            if (User.IsInRole("Lärare"))
            {
                return RedirectToAction("Index", "Teacher", new { @courseId = backToId });
            }
            else
            {
                return RedirectToAction("Index", "Student", new { @corseId = backToId });
            }


            //return View(document);
        }

        // GET: Documents/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Document document = db.Documents.Find(id);
            if (document == null)
            {
                return HttpNotFound();
            }
            ViewBag.ActivityId = new SelectList(db.Activities, "Id", "Name", document.ActivityId);
            ViewBag.CourseId = new SelectList(db.Courses, "Id", "Name", document.CourseId);
            ViewBag.DocTypeId = new SelectList(db.DocTypes, "Id", "Extension", document.DocTypeId);
            ViewBag.ModuleId = new SelectList(db.Modules, "Id", "Name", document.ModuleId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName", document.UserId);
            return View(document);
        }

        // POST: Documents/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Description,Name,FileName,CreationTime,UserId,CourseId,ModuleId,ActivityId,DocTypeId")] Document document)
        {
            if (ModelState.IsValid)
            {
                db.Entry(document).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ActivityId = new SelectList(db.Activities, "Id", "Name", document.ActivityId);
            ViewBag.CourseId = new SelectList(db.Courses, "Id", "Name", document.CourseId);
            ViewBag.DocTypeId = new SelectList(db.DocTypes, "Id", "Extension", document.DocTypeId);
            ViewBag.ModuleId = new SelectList(db.Modules, "Id", "Name", document.ModuleId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName", document.UserId);
            return View(document);
        }

        // GET: Documents/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Document document = db.Documents.Find(id);
            if (document == null)
            {
                return HttpNotFound();
            }
            return View(document);
        }

        // POST: Documents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Document document = db.Documents.Find(id);
            db.Documents.Remove(document);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public void ActivityHasDocuments(Activity activity)
        {
            TempData.Remove("hoursR");
            TempData.Remove("minR");
            TempData.Remove("days");
            TempData.Remove("hours");
            TempData.Remove("min");

            if (activity.DeadLine != null)
            {
                var days = activity.DeadLine.Value.Subtract(DateTime.Now).Days;
                var hours = activity.DeadLine.Value.Subtract(DateTime.Now).Hours;
                var min = activity.DeadLine.Value.Subtract(DateTime.Now).Minutes;
                if (days <= 0)
                {
                    TempData.Add("hoursR", hours);
                    TempData.Add("minR", min);
                }
                else
                {
                    TempData.Add("days", days);
                    TempData.Add("hours", hours);
                    TempData.Add("min", min);
                }
            }
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
