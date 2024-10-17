using System;
using System.Collections.Generic;

namespace VJN.Models
{
    public partial class JobCategory
    {
        public JobCategory()
        {
            PostJobs = new HashSet<PostJob>();
        }

        public int JobCategoryId { get; set; }
        public string? JobCategoryName { get; set; }

        public virtual ICollection<PostJob> PostJobs { get; set; }
    }
}
