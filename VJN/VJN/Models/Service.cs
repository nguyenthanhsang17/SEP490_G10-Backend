using System;
using System.Collections.Generic;

namespace VJN.Models
{
    public partial class Service
    {
        public int ServiceId { get; set; }
        public int? UserId { get; set; }
        public int? NumberPosts { get; set; }
        public int? NumberPostsUrgentRecruitment { get; set; }
        public int? IsFindJobseekers { get; set; }
        public DateTime? ExpirationDate { get; set; }

        public virtual User? User { get; set; }
    }
}
