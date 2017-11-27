using Laja.Models;
using Laja.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Web.Mvc;

namespace Laja.Controllers
{
    public class UserController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();




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
                    return View(newUser);
                }

                var findNewUser = userManager.FindByEmail(newUser.Email);

                if (findNewUser != null)
                {
                    var resultRole = userManager.AddToRole(findNewUser.Id, User.Role);
                    if (resultRole.Succeeded == false)
                    {
                        return View(newUser);
                    }

                    //User and Role definition OK

                    //If role Elev redirect to 
                    //else (Lärare redirect to 

                    if (User.Role == "Elev")
                    {
                        return RedirectToAction("Details", "Courses" , new { id = User.CourseId });
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