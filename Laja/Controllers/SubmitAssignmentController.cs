using Laja.Models;
using Laja.ViewModels;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace Laja.Controllers
{
    [Authorize(Roles = "Lärare")]
    public class SubmitAssignmentController : Controller
    {

        private readonly ApplicationDbContext db;

        public SubmitAssignmentController()
        {
            db = new ApplicationDbContext();
        }

        // GET: SubmitAssignment
        public PartialViewResult Index(int activityId)
        {

            var activity = db.Activities.Find(activityId);

            var moduleId = db.Activities.Find(activityId).ModuleId;
            var courseId = db.Modules.Find(moduleId).CourseId;

            var activityDocs = activity.Documents;

            var course = db.Courses
                .Where(c => c.Id == courseId)
                .Include(c => c.Students)
                .Include(c => c.Documents)
                .FirstOrDefault();

            //var assignmentDocuments = db.Documents.Where(d => d.ActivityId == activityId);

            var assignmentSubmit = new AssignmentSubmitViewModel()
            {
                ActivityId = activityId,
                ActivityName = activity.Name,
                CourseId = course.Id,
                Students = course.Students,
                Documents = activityDocs
            };

            return PartialView("_SubmitAssignment", assignmentSubmit);
        }
    }
}