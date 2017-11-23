using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Laja.Models;

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

        
    }
}