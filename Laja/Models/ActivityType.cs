using System.Collections.Generic;

namespace Laja.Models
{
    public class ActivityType
    {
        public int Id { get; set; }
        public string Name { get; set; }


        //Navigation

        public  ICollection<Activity> Activities { get; set; }

    }
}