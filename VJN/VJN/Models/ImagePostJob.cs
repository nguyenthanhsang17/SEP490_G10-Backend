using System;
using System.Collections.Generic;

namespace VJN.Models
{
    public partial class ImagePostJob
    {
        public int ImageJobId { get; set; }
        public int? PostId { get; set; }
        public int? ImageId { get; set; }

        public virtual MediaItem? Image { get; set; }
        public virtual PostJob? Post { get; set; }
    }
}
