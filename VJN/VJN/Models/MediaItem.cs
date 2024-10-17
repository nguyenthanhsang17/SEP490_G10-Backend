using System;
using System.Collections.Generic;

namespace VJN.Models
{
    public partial class MediaItem
    {
        public MediaItem()
        {
            Blogs = new HashSet<Blog>();
            ImagePostJobs = new HashSet<ImagePostJob>();
            RegisterEmployerMedia = new HashSet<RegisterEmployerMedium>();
            ReportMedia = new HashSet<ReportMedium>();
            Users = new HashSet<User>();
        }

        public int Id { get; set; }
        public string? Url { get; set; }
        public bool? Status { get; set; }

        public virtual ICollection<Blog> Blogs { get; set; }
        public virtual ICollection<ImagePostJob> ImagePostJobs { get; set; }
        public virtual ICollection<RegisterEmployerMedium> RegisterEmployerMedia { get; set; }
        public virtual ICollection<ReportMedium> ReportMedia { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
