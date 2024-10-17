using System;
using System.Collections.Generic;

namespace VJN.Models
{
    public partial class Blog
    {
        public int BlogId { get; set; }
        public string? BlogTitle { get; set; }
        public string? BlogDescription { get; set; }
        public DateTime? CreateDate { get; set; }
        public int? AuthorId { get; set; }
        public int? Thumbnail { get; set; }

        public virtual User? Author { get; set; }
        public virtual MediaItem? ThumbnailNavigation { get; set; }
    }
}
