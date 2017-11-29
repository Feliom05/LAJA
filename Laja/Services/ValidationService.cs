using Laja.Models;
using System;
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
        #region Courses
        public bool CanDeleteCourse(Course course)
        {
            if (CourseHasMoules(course))
                return false;
            if (CourseHasStudents(course))
                return false;
            if (CourseHasDocuments(course))
                return false;
            return true;
        }
        public bool CourseHasMoules(Course course)
        {
            var modules = 0;
            modules = db.Courses.Find(course.Id).Modules.Count();
            return (modules > 0) ? true : false;
        }
        public bool CourseHasStudents(Course course)
        {
            var students = 0;
            students = db.Courses.Find(course.Id).Students.Count();
            return (students > 0) ? true : false;
        }
        public bool CourseHasDocuments(Course course)
        {
            var documents = 0;
            documents = db.Courses.Find(course.Id).Documents.Count();
            return (documents > 0) ? true : false;
        }
        public bool UniqName(Course course)
        {
            bool isUniqe = false;
            isUniqe = db.Courses.Any(c => c.Name.ToLower() == course.Name.ToLower() && c.Id != course.Id);
            return isUniqe;
        }
        public bool CheckPeriod(Course course)
        {
            if (course.StartDate == null || course.EndDate == null)
                return false;
            return EndDateIsAfterStartDate(course.StartDate, course.EndDate);
        }
        public bool CheckCoursePeriodAgainstModules(Course course)
        {
            if (course.StartDate == null || course.EndDate == null)
                return false;
            var savedModuels = db.Courses.Find(course.Id).Modules.ToList();
            foreach (var module in savedModuels)
            {
                if (!EndDateIsEqualAfterStartDate(module.StartDate, course.StartDate) || !EndDateIsEqualAfterStartDate(module.EndDate, course.EndDate))
                    return false;
            }
            return true;
        }
        #endregion
        #region Modules
        public bool CanDeleteModule(Module module)
        {
            if (ModuleHasActivities(module))
                return false;
            if (ModuleHasDocuments(module))
                return false;
            return true;
        }
        public bool ModuleHasActivities(Module module)
        {
            var activities = 0;
            activities = db.Modules.Find(module.Id).Activities.Count();
            return (activities > 0) ? true : false;
        }
        public bool ModuleHasDocuments(Module module)
        {
            var documents = 0;
            documents = db.Modules.Find(module.Id).Documents.Count();
            return (documents > 0) ? true : false;
        }
        public bool UniqName(Module module)
        {
            bool isUniqe = false;
            isUniqe = db.Modules.Any(c => c.Name.ToLower() == module.Name.ToLower() && c.CourseId != module.CourseId);
            return isUniqe;
        }
        public bool CheckModulePeriodAgainstActivities(Module module)
        {
            if (module.StartDate == null || module.EndDate == null)
                return false;
            foreach (var activity in module.Activities)
            {
                if (!EndDateIsEqualAfterStartDate(activity.StartDate, module.StartDate) || !EndDateIsEqualAfterStartDate(activity.EndDate, module.EndDate))
                    return false;
            }
            return true;
        }
        public bool CheckModulePeriodAgainstCourse(Module module)
        {
            var targetModule = db.Courses.Find(module.CourseId);
            if (EndDateIsEqualAfterStartDate(targetModule.StartDate, module.StartDate) && EndDateIsEqualAfterStartDate(module.EndDate, targetModule.EndDate))
                return true;
            else
                return false;
        }
        #endregion
        #region Activity
        public bool CanDeleteActivity(Activity activity)
        {
            if (ActivityHasDocuments(activity))
                return false;
            return true;
        }
        public bool ActivityHasDocuments(Activity activity)
        {
            var documents = 0;
            documents = db.Activities.Find(activity.Id).Documents.Count();
            return (documents > 0) ? true : false;
        }
        public bool UniqName(Activity activity)
        {
            bool isUniqe = false;
            isUniqe = db.Activities.Any(c => c.Name.ToLower() == activity.Name.ToLower() && c.ModuleId != activity.ModuleId);
            return isUniqe;
        }
        public bool CheckPeriod(Activity activity)
        {
            if (activity.StartDate == null || activity.EndDate == null)
                return false;
            return EndDateIsAfterStartDate(activity.StartDate, activity.EndDate);
        }
        public bool CheckActivityPeriodAgainstModule(Activity activity)
        {
            var module = db.Modules.Find(activity.ModuleId);
            if (EndDateIsEqualAfterStartDate(module.StartDate, activity.StartDate) && EndDateIsEqualAfterStartDate(activity.EndDate, module.EndDate))
                return true;
            else
                return false;
        }
        #endregion
        private bool EndDateIsEqualAfterStartDate(DateTime startDate, DateTime endDate)
        {
            if (endDate.Subtract(startDate).TotalDays < 0)
                return false;
            else
                return true;
        }
        private bool EndDateIsAfterStartDate(DateTime startDate, DateTime endDate)
        {
            if (endDate.Subtract(startDate).TotalDays <= 0)
                return false;
            else
                return true;
        }

    }
}