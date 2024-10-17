using System;
using System.Collections.Generic;

namespace VJN.Models
{
    public partial class Chat
    {
        public int ChatId { get; set; }
        public int? SendFromId { get; set; }
        public int? SendToId { get; set; }
        public string? Message { get; set; }
        public DateTime? SendTime { get; set; }

        public virtual User? SendFrom { get; set; }
        public virtual User? SendTo { get; set; }
    }
}
