using System;
using System.Collections.Generic;

namespace VJN.Models
{
    public partial class Cv
    {
        public Cv()
        {
            ItemOfCvs = new HashSet<ItemOfCv>();
        }

        public int CvId { get; set; }
        public int? UserId { get; set; }

        public virtual User? User { get; set; }
        public virtual ICollection<ItemOfCv> ItemOfCvs { get; set; }
    }
}
