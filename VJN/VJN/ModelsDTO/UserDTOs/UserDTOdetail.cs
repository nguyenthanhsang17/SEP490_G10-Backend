﻿using VJN.Models;

namespace VJN.ModelsDTO.UserDTOs
{
    public class UserDTOdetail
    {
        public int UserId { get; set; }
        public string? Email { get; set; }
        public int? Avatar { get; set; }
        public string? FullName { get; set; }
        public string? UserName { get; set; }
        public int? Age { get; set; }
        public string? Phonenumber { get; set; }
        public int? CurrentJob { get; set; }
        public string? Description { get; set; }
        public string? Address { get; set; }
        public bool? Gender { get; set; }
        public virtual ICollection<Cv> Cvs { get; set; }
    }
}
