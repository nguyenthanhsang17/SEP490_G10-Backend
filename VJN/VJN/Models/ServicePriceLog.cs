using System;
using System.Collections.Generic;

namespace VJN.Models
{
    public partial class ServicePriceLog
    {
        public int ServicePriceLogId { get; set; }
        public int? UserId { get; set; }
        public int? ServicePriceId { get; set; }
        public DateTime? RegisterDate { get; set; }
        public int? NumberPostRemaining { get; set; }
        public int? NumberPostsUrgentRecruitmentRemaining { get; set; }
        public int? Status { get; set; }

        public virtual ServicePriceList? ServicePrice { get; set; }
        public virtual User? User { get; set; }
    }
}
