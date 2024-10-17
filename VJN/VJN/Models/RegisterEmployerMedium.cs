using System;
using System.Collections.Generic;

namespace VJN.Models
{
    public partial class RegisterEmployerMedium
    {
        public int RegisterEmployerMedia { get; set; }
        public int? RegisterEmployerId { get; set; }
        public int? MediaId { get; set; }

        public virtual MediaItem? Media { get; set; }
        public virtual RegisterEmployer? RegisterEmployer { get; set; }
    }
}
