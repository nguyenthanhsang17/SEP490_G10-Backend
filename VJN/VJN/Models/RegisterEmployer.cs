using System;
using System.Collections.Generic;

namespace VJN.Models
{
    public partial class RegisterEmployer
    {
        public RegisterEmployer()
        {
            RegisterEmployerMedia = new HashSet<RegisterEmployerMedium>();
        }

        public int RegisterEmployerId { get; set; }
        public int? UserId { get; set; }
        public string? BussinessName { get; set; }
        public string? BussinessAddress { get; set; }
        public DateTime? CreateDate { get; set; }
        public int? Status { get; set; }

        public virtual User? User { get; set; }
        public virtual ICollection<RegisterEmployerMedium> RegisterEmployerMedia { get; set; }
    }
}
