using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Laja.Models;
using System.IO;
using Microsoft.AspNet.Identity;

namespace Laja.Controllers
{
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
        public ActionResult Create()
        {
            ViewBag.ActivityId = new SelectList(db.Activities, "Id", "Name");
            ViewBag.CourseId = new SelectList(db.Courses, "Id", "Name");
            ViewBag.DocTypeId = new SelectList(db.DocTypes, "Id", "Extension");
            ViewBag.ModuleId = new SelectList(db.Modules, "Id", "Name");
            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName");
            return View();
        }

        // POST: Documents/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Description,Name,FileName,CreationTime,UserId,CourseId,ModuleId,ActivityId,DocTypeId")] Document document, HttpPostedFileBase upload1)
        {
            foreach (string upload in Request.Files)
            {
                if (Request.Files[upload].FileName != "")
                {
                    string path = AppDomain.CurrentDomain.BaseDirectory + "/App_Data/Documents/";
                    string filename = Path.GetFileName(Request.Files[upload].FileName);                   
                    document.FileName = filename + "_" + DateTime.Now.ToString();
                    document.CreationTime = DateTime.Now;
                    var manager = new UserManager<ApplicationUser>(new Microsoft.AspNet.Identity.EntityFramework.UserStore<ApplicationUser>(new ApplicationDbContext()));
                    var currentUser = manager.FindById(User.Identity.GetUserId()).UserName;
                    document.UserId = manager.FindById(User.Identity.GetUserId()).Id;                    
                    var folder = Directory.CreateDirectory(Path.Combine(path, currentUser)).FullName;
                    Request.Files[upload].SaveAs(Path.Combine(folder, filename));                    
                    var filePath= Path.Combine(folder, filename);
                    String RelativePath = filePath.Replace(Request.ServerVariables["APPL_PHYSICAL_PATH"], String.Empty);
                    document.Name = RelativePath;
                    db.Documents.Add(document);
                    db.SaveChanges();
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
