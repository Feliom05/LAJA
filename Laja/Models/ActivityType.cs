using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Laja.Models
{
    public class ActivityType
    {
        public int Id { get; set; }

        [Display(Name = "Aktivitetstypsnamn")]
        public string Name { get; set; }


        //Navigation

        public  ICollection<Activity> Activities { get; set; }

    }
}