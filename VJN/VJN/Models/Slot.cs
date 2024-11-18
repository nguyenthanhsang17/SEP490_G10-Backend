using System;
using System.Collections.Generic;

namespace VJN.Models
{
    public partial class Slot
    {
        public Slot()
        {
            JobSchedules = new HashSet<JobSchedule>();
        }

        public int SlotId { get; set; }
        public int? PostId { get; set; }

        public virtual PostJob? Post { get; set; }
        public virtual ICollection<JobSchedule> JobSchedules { get; set; }
    }
}
