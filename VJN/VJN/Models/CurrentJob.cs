using System;
using System.Collections.Generic;

namespace VJN.Models
{
    public partial class CurrentJob
    {
        public CurrentJob()
        {
            Users = new HashSet<User>();
        }

        public int CurrentJobId { get; set; }
        public string? JobName { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
