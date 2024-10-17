using System;
using System.Collections.Generic;

namespace VJN.Models
{
    public partial class BanLogPostJob
    {
        public int BanLogPostJob1 { get; set; }
        public int? PostId { get; set; }
        public int? AdminId { get; set; }
        public string? Reason { get; set; }
        public int? Status { get; set; }

        public virtual User? Admin { get; set; }
        public virtual PostJob? Post { get; set; }
    }
}
