namespace VJN.ModelsDTO.UserDTOs
{
    public class UserUpdateDTO
    {
        public IFormFile? Avatar { get; set; }
        public string? FullName { get; set; }
        public int? Age { get; set; }
        public string? Phonenumber { get; set; }
        public int? CurrentJob { get; set; }
        public string? Description { get; set; }
        public string? Address { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public bool? Gender { get; set; }
    }
}
