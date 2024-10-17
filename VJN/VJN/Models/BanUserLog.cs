using System;
using System.Collections.Generic;

namespace VJN.Models
{
    public partial class BanUserLog
    {
        public int BanId { get; set; }
        public int? UserId { get; set; }
        public int? AdminId { get; set; }
        public string? BanReason { get; set; }
        public DateTime? BanDate { get; set; }
        public DateTime? UnbanDate { get; set; }
        public int? Status { get; set; }

        public virtual User? Admin { get; set; }
        public virtual User? User { get; set; }
    }
}
