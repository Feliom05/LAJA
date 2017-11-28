using Laja.Models;
using System.Linq;
using System;

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
            isUniqe = db.Courses.Any(c => c.Name.ToLower() == course.Name.ToLower() && c.Id != course.Id);
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
            var targetCourse = db.Courses.Find(module.CourseId);
            if (EndDateIsAfterStartDate(targetCourse.StartDate, module.StartDate) && EndDateIsAfterStartDate(module.EndDate, targetCourse.EndDate))
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

        private bool EndDateIsEqualAfterStartDate(DateTime startDate, DateTime endDate)
        {
            if (endDate.Subtract(startDate).TotalDays < 0)
                return false;
            else
                return true;
        }

        public string CreateUniqFileName(string fileName)
        {
            return "";
        }

        public bool CheckCoursePeriodAgainstModules(Course course)
        {
            if (course.StartDate == null || course.EndDate == null)
                return false;

            var savedModules = db.Courses.Find(course.Id).Modules.ToList();
            
            foreach (var module in savedModules)
            {
                if (!EndDateIsEqualAfterStartDate(course.StartDate, module.StartDate) || (!EndDateIsEqualAfterStartDate(module.EndDate, course.EndDate)))                
                    return false;                
            }
            return true;
        }
    }
}