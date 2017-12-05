using Laja.Models;
using Laja.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Web.Mvc;

namespace Laja.Controllers
{
    [Authorize(Roles = "Lärare")]
    public class UserController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }


        // GET: User
        public ActionResult Create(int? courseId, string role)
        {
            var user = new UserViewModel()
            {
                CourseId = courseId,
                Role = role
            };

            return View(user);
        }

        // POST: Courses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "FirstName, LastName, Email, PassWord, CourseId, Role")] UserViewModel User)
        {

            var userStore = new UserStore<ApplicationUser>(db);
            var userManager = new UserManager<ApplicationUser>(userStore);

            if (ModelState.IsValid)
            {
                var newUser = new ApplicationUser
                {
                    FirstName = User.FirstName,
                    LastName = User.LastName,
                    UserName = User.Email,
                    Email = User.Email,
                    CourseId = User.CourseId
                };

                var result = userManager.Create(newUser, User.PassWord);

                if (result.Succeeded == false)
                {
                    AddErrors(result);
                    return View(User);
                }

                var findNewUser = userManager.FindByEmail(newUser.Email);

                if (findNewUser != null)
                {
                    var resultRole = userManager.AddToRole(findNewUser.Id, User.Role);
                    if (resultRole.Succeeded == false)
                    {
                        AddErrors(resultRole);
                        return View(User);
                    }

                    //User and Role definition OK

                    //If role Elev redirect to Courses/Details
                    //else (Lärare redirect to Courses/Index

                    if (User.Role == "Elev")
                    {
                        return RedirectToAction("Index", "Teacher", new { CourseId = User.CourseId });
                    }
                    else
                    {
                        return RedirectToAction("Index", "Courses");
                    }
                }
            }

            return View(User);
        }
    }
}