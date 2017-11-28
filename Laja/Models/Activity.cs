using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Laja.Models
{
    public class Activity
    {
        public int Id { get; set; }

        [Display(Name = "Aktivitetsnamn")]
        [Required]
        [StringLength(255)]
        [Index("IX_UniqeActivityName", 1, IsUnique = true)]
        public string Name { get; set; }

        [Index("IX_UniqeActivityName", 2, IsUnique = true)]
        public int ModuleId { get; set; }

        [Display(Name = "Aktivitets beskrivning")]
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

        [Display(Name ="Deadline")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime? DeadLine { get; set; }

        public bool SubmitRequired { get; set; }

        public int ActivityTypeId { get; set; }

        //Navigation

        public virtual Module Module { get; set; }
        public virtual ActivityType ActivityType { get; set; }


    }
}