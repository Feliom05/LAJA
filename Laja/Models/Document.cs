﻿using System;
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

        public string Description { get; set; }
        public string Name { get; set; }

        [Required]
        [StringLength(255)]
        [Index("IX_UniqeFileName", IsUnique = true)]
        public string FileName { get; set; }
        public DateTime CreationTime { get; set; }

        
        public string UserId { get; set; }

        //public string FeedBack { get; set; }

        
        public int? CourseId { get; set; }
        public int? ModuleId { get; set; }
        public int? ActivityId { get; set; }
        
        public int DocTypeId { get; set; }

        public ApplicationUser User { get; set; }
        public Course Course { get; set; }
        public Module Module { get; set; }
        //public Activity Activity { get; set; }


    }
}