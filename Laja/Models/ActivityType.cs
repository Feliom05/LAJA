using System.Collections.Generic;

namespace Laja.Models
{
    public class ActivityType
    {
        public int Id { get; set; }
        public string Name { get; set; }

        //public int ActivityId { get; set; }

        //Navigation

        public ICollection<Activity> Activites { get; set; }

    }
}