using System;
using System.Collections.Generic;

namespace VJN.Models
{
    public partial class ReportMedium
    {
        public int ReportImageId { get; set; }
        public int? ReportId { get; set; }
        public int? ImageId { get; set; }

        public virtual MediaItem? Image { get; set; }
        public virtual Report? Report { get; set; }
    }
}
