using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Laja.Models
{
    public class Module
    {
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        [Index("IX_UniqeModelName", 1, IsUnique =true)]
        public string Name { get; set; }

        [Index("IX_UniqeModelName", 2, IsUnique = true)]
        public int CourseId { get; set; }

        public string Description { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }

        

        //Navigation

        public Course Course { get; set; }
        public virtual ICollection<Activity> Activities { get; set; }
        public virtual ICollection<Document> Documents { get; set; }

    }
}