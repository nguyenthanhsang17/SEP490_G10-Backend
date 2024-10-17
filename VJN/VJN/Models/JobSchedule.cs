using System;
using System.Collections.Generic;

namespace VJN.Models
{
    public partial class JobSchedule
    {
        public JobSchedule()
        {
            WorkingHours = new HashSet<WorkingHour>();
        }

        public int ScheduleId { get; set; }
        public int? SlotId { get; set; }
        public int? DayOfWeek { get; set; }

        public virtual Slot? Slot { get; set; }
        public virtual ICollection<WorkingHour> WorkingHours { get; set; }
    }
}
