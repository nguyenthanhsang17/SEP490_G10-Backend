namespace VJN.ModelsDTO.UserDTOs
{
    public class UserDTOinRegisterEmployer
    {
        public int UserId { get; set; }
        public string? Email { get; set; }
        public string? AvatarURL { get; set; }
        public string? FullName { get; set; }
        public int? Age { get; set; }
        public string? Phonenumber { get; set; }
        public bool? Gender { get; set; }
    }
}
