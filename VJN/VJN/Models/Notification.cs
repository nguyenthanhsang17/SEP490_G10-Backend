using System;
using System.Collections.Generic;

namespace VJN.Models
{
    public partial class Notification
    {
        public int NotifycationId { get; set; }
        public int? UserId { get; set; }
        public string? NotifycationContent { get; set; }

        public virtual User? User { get; set; }
    }
}
