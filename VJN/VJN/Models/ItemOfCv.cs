using System;
using System.Collections.Generic;

namespace VJN.Models
{
    public partial class ItemOfCv
    {
        public int ItemOfCvId { get; set; }
        public int? CvId { get; set; }
        public string? ItemName { get; set; }
        public string? ItemDescription { get; set; }

        public virtual Cv? Cv { get; set; }
    }
}
