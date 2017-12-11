using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Laja.Models
{
    public class Document
    {
        public int Id { get; set; }

        [Display(Name = "Beskrivning:")]
        [DisplayFormat(NullDisplayText = "-")]
        public string Description { get; set; }

        public bool DeadLineFixed { get; set; }

        [Display(Name = "Dela")]
        public bool IsShared { get; set; }

        [Display(Name = "Dokumentets plats:")]
        public string Path { get; set; }

        [Required]
        [StringLength(255)]
        [Index("IX_UniqeFileName", IsUnique = true)]
        public string FileName { get; set; }
        public DateTime CreationTime { get; set; }
        public string UserId { get; set; }

        [Display(Name = "Återkoppling")]
        [DisplayFormat(NullDisplayText = "-")]
        public string FeedBack { get; set; }
        
        public int? CourseId { get; set; }        
        public int? ModuleId { get; set; }        
        public int? ActivityId { get; set; }
        public int DocTypeId { get; set; }

        public virtual ApplicationUser User { get; set; }
        public virtual Course Course { get; set; }
        public virtual Module Module { get; set; }
        public virtual Activity Activity { get; set; }
        public virtual DocType DocType { get; set; }


    }
}