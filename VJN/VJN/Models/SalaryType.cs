using System;
using System.Collections.Generic;

namespace VJN.Models
{
    public partial class SalaryType
    {
        public SalaryType()
        {
            PostJobs = new HashSet<PostJob>();
        }

        public int SalaryTypesId { get; set; }
        public string TypeName { get; set; } = null!;

        public virtual ICollection<PostJob> PostJobs { get; set; }
    }
}
