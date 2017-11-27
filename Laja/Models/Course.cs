using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Laja.Models
{
    public class Course
    {
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        [Index(IsUnique = true)]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }

        //nav
        public virtual ICollection<ApplicationUser> Students { get; set; }
        public virtual ICollection<Module> Modules { get; set; }

    }
}