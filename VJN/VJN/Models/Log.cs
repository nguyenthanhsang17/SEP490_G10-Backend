﻿using System;
using System.Collections.Generic;

namespace VJN.Models
{
    public partial class Log
    {
        public int LogId { get; set; }
        public int? UserId { get; set; }
        public string? Action { get; set; }
        public string? Description { get; set; }
        public DateTime? Time { get; set; }

        public virtual User? User { get; set; }
    }
}
