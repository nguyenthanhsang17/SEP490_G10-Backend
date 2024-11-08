namespace VJN.ModelsDTO.JobSeekerDTOs
{
    public class JobSeekerDTO
    {
        public int UserId { get; set; }
        public string? AvatarURL { get; set; }
        public string? FullName { get; set; }
        public int? Age { get; set; }
        public string? CurrentJob { get; set; }
        public string? Address { get; set; }
        public bool? Gender { get; set; }
        public string? DescriptionFavorite { get; set; }
        public int? NumberAppliedAccept {  get; set; }
        public int? NumberApplied { get; set; }
    }
}
