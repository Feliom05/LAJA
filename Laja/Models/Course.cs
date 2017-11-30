using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Laja.Models
{
    public class Course
    {
        //public Course()
        //{
        //    Students = new HashSet<ApplicationUser>();
        //    Modules = new HashSet<Module>();
        //}
        public int Id { get; set; }

        [Display(Name = "Kursnamn")]
        [Required]
        [StringLength(255)]
        [Index(IsUnique = true)]
        public string Name { get; set; }

        [Display(Name = "Kursbeskrivning")]
        public string Description { get; set; }

        [Display(Name = "Startdatum")]
        [Required]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }

        [Display(Name = "Slutdatum")]
        [Required]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }

        //nav
        public virtual DocType DocumentType { get; set; }
        public virtual ICollection<ApplicationUser> Students { get; set; }
        public virtual ICollection<Module> Modules { get; set; }
        public virtual ICollection<Document> Documents { get; set; }

    }
}