using System;
using System.Collections.Generic;

namespace VJN.Models
{
    public partial class WishJob
    {
        public int WishJobId { get; set; }
        public int? PostJobId { get; set; }
        public int? JobSeekerId { get; set; }

        public virtual User? JobSeeker { get; set; }
        public virtual PostJob? PostJob { get; set; }
    }
}
