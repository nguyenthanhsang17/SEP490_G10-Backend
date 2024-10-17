using System;
using System.Collections.Generic;

namespace VJN.Models
{
    public partial class FavoriteList
    {
        public int FavoriteListId { get; set; }
        public int? EmployerId { get; set; }
        public int? JobSeekerId { get; set; }
        public string? Description { get; set; }

        public virtual User? Employer { get; set; }
        public virtual User? JobSeeker { get; set; }
    }
}
