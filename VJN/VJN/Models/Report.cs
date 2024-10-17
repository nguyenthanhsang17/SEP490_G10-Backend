using System;
using System.Collections.Generic;

namespace VJN.Models
{
    public partial class Report
    {
        public Report()
        {
            ReportMedia = new HashSet<ReportMedium>();
        }

        public int ReportId { get; set; }
        public int? JobSeekerId { get; set; }
        public string? Reason { get; set; }
        public int? PostId { get; set; }
        public DateTime? CreateDate { get; set; }
        public int? Status { get; set; }

        public virtual User? JobSeeker { get; set; }
        public virtual PostJob? Post { get; set; }
        public virtual ICollection<ReportMedium> ReportMedia { get; set; }
    }
}
