using System;
using System.Collections.Generic;

namespace VJN.Models
{
    public partial class ApplyJob
    {
        public int Id { get; set; }
        public int? PostId { get; set; }
        public int? JobSeekerId { get; set; }
        public int? CvId { get; set; }
        public DateTime? ApplyDate { get; set; }
        public string? Reason { get; set; }
        public int? Status { get; set; }

        public virtual Cv? Cv { get; set; }
        public virtual User? JobSeeker { get; set; }
        public virtual PostJob? Post { get; set; }
    }
}
