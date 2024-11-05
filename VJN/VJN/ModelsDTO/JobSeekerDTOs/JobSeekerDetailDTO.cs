using VJN.ModelsDTO.CvDTOs;

namespace VJN.ModelsDTO.JobSeekerDTOs
{
    public class JobSeekerDetailDTO
    {
        public int UserId { get; set; }
        public string? Email { get; set; }
        public string? AvatarURL { get; set; }
        public string? FullName { get; set; }
        public int? Age { get; set; }
        public string? Phonenumber { get; set; }
        public string? CurrentJob { get; set; }
        public string? Description { get; set; }
        public string? Address { get; set; }
        public bool? Gender { get; set; }
        public int? RoleId { get; set; }
        public int NumberAppiled {  get; set; }
        public int NumberAppiledAccept {  get; set; }
        public IEnumerable<CvDTODetail> CvDTOs { get; set; }

    }
}
