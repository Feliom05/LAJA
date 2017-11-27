using System.Collections.Generic;

namespace Laja.Models
{
    public class ActivityType
    {
        public int Id { get; set; }
        public string Name { get; set; }


        //Navigation

        public virtual ICollection<Activity> Activities { get; set; }

    }
}