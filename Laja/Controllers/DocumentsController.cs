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
                        throw new Exception("Extension not allowed!");
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

            
            return View(document);
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
