namespace VJN.ModelsDTO.UserDTOs
{
    public class UserDTOforList
    {
        public int UserId { get; set; }
        public int Apply_id { get; set; }
        public int? Avatar { get; set; }
        public string? AvatarURL { get; set; }
        public string FullName { get; set; }
        public int Age { get; set; }
        public string JobName { get; set; }
        public bool? Gender { get; set; }
    }
}
