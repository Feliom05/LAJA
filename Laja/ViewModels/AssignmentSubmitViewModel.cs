using Laja.Models;
using System.Collections.Generic;

namespace Laja.ViewModels
{
    public class AssignmentSubmitViewModel
    {
        public int ActivityId { get; set; }

        public string ActivityName { get; set; }

        public int CourseId { get; set; }

        public virtual ICollection<ApplicationUser> Students { get; set; }

        public virtual ICollection<Document> Documents { get; set; }
    }
}