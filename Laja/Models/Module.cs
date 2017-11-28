using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Laja.Models
{
    public class Module
    {
        public int Id { get; set; }

        [Display(Name = "Modulnamn")]
        [Required]
        [StringLength(255)]
        [Index("IX_UniqeModelName", 1, IsUnique = true)]
        public string Name { get; set; }

        [Index("IX_UniqeModelName", 2, IsUnique = true)]
        public int CourseId { get; set; }

        [Display(Name = "Modulbeskrivning")]
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



        //Navigation

        public virtual Course Course { get; set; }
        public virtual ICollection<Activity> Activities { get; set; }

    }
}