using Laja.Models;
using System.Linq;

namespace Laja.Services
{
    public class ValidationService
    {
        ApplicationDbContext db = new ApplicationDbContext();
        public ValidationService(ApplicationDbContext db)
        {
            this.db = db;
        }

        public bool UniqName(Course course)
        {
            bool isUniqe = false;
            isUniqe = db.Courses.Any(c => c.Name.ToLower() == course.Name.ToLower());
            return isUniqe;
        }

        public bool UniqName(Module module)
        {
            bool isUniqe = false;
            isUniqe = db.Modules.Any(c => c.Name.ToLower() == module.Name.ToLower() && c.CourseId == module.CourseId);
            return isUniqe;
        }

        public bool CheckPeriod(Course course)
        {
            if (course.StartDate == null && course.EndDate == null)
                return false;
            return EndDateIsAfterStartDate(course.StartDate, course.EndDate);
           
        }
        //public bool CheckPeriod(Activity activity)
        //{
        //    if (activity.StartDate == null && activity.EndDate == null)
        //        return false;
        //    return EndDateIsAfterStartDate(activity.StartDate, activity.EndDate);

        //}

        public bool CheckModulePeriodAgainstCourse(Module module)
        {
            if (EndDateIsAfterStartDate(module.Course.StartDate, module.StartDate) && EndDateIsAfterStartDate(module.EndDate, module.Course.EndDate))
                return true;
            else
               return false;
        }

        //public bool CheckActivityPeriodAgainstModule(Activity activity)
        //{
        //    if (EndDateIsAfterStartDate(activity.Module.StartDate, activity.StartDate) && EndDateIsAfterStartDate(activity.EndDate, activity.Module.EndDate))
        //        return true;
        //    else
        //        return false;
        //}

        private bool EndDateIsAfterStartDate(DateTime startDate, DateTime endDate)
        {
            if (endDate.Subtract(startDate).TotalDays <= 0)
                return false;
            else
                return true;
        }

        public string CreateUniqFileName(string fileName)
        {

        }

    }
}