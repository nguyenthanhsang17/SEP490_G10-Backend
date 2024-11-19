using VJN.ModelsDTO.PostJobDTOs;

namespace VJN.ModelsDTO.EmployerDTOs
{
    public class EmployerDTO
    {
        public int? Avatar { get; set; }
        public string? FullName { get; set; }
        public int? Age { get; set; }
        public int? CurrentJob { get; set; }
        public string? Description { get; set; }
        public string? Address { get; set; }
        public bool? Gender { get; set; }
        public List<JobSearchResult> PostJobAuthors { get; set; }
    }
}
