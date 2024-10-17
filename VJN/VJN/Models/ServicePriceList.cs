using System;
using System.Collections.Generic;

namespace VJN.Models
{
    public partial class ServicePriceList
    {
        public ServicePriceList()
        {
            ServicePriceLogs = new HashSet<ServicePriceLog>();
        }

        public int ServicePriceId { get; set; }
        public int? NumberPosts { get; set; }
        public int? NumberPostsUrgentRecruitment { get; set; }
        public decimal? Price { get; set; }

        public virtual ICollection<ServicePriceLog> ServicePriceLogs { get; set; }
    }
}
