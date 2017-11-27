using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Laja.Models
{
    public class DocType
    {
        public int Id { get; set; }

        [Required]
        [StringLength(10)]
        [Index(IsUnique = true)]
        public string Extension { get; set; }

        public string Name { get; set; }

        public virtual ICollection<Document> Documents { get; set; }
    }
}