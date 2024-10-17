using System;
using System.Collections.Generic;

namespace VJN.Models
{
    public partial class WorkingHour
    {
        public int WorkingHourId { get; set; }
        public int? ScheduleId { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }

        public virtual JobSchedule? Schedule { get; set; }
    }
}
