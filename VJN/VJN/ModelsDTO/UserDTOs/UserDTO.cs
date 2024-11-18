namespace VJN.ModelsDTO.UserDTOs
{
    public class UserDTO
    {
        public int UserId { get; set; }
        public string? Email { get; set; }
        public int? Avatar { get; set; }
        public string? AvatarURL { get; set; }
        public string? FullName { get; set; }
        public int? Age { get; set; }
        public string? Phonenumber { get; set; }
        public int? CurrentJob { get; set; }
        public string? JobName { get; set; }
        public string? Description { get; set; }
        public string? Address { get; set; }
        public decimal? Balance { get; set; }
        public int? Status { get; set; }
        public bool? Gender { get; set; }
        public DateTime? SendCodeTime { get; set; }
        public string? VerifyCode { get; set; }
        public int? RoleId { get; set; }
        public string RoleName { get; set; }
        public int? RegisterEmployerStatus {  get; set; }
        public string? Reason {  get; set; } 
    }
}
