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
        public int? NumberPosts { get; set; }
        public int? NumberPostsUrgentRecruitment { get; set; }
        public int? IsFindJobseekers { get; set; }
        public int? DurationsMonth { get; set; }
        public int? ExpirationMonth { get; set; }
        public int? Status { get; set; }

        public virtual ServicePriceList? ServicePrice { get; set; }
        public virtual User? User { get; set; }
    }
}
