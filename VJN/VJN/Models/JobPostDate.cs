using System;
using System.Collections.Generic;

namespace VJN.Models
{
    public partial class JobPostDate
    {
        public int EventDateId { get; set; }
        public int? PostId { get; set; }
        public DateTime? EventDate { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }

        public virtual PostJob? Post { get; set; }
    }
}
