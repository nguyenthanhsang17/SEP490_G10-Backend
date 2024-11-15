using VJN.ModelsDTO.JobPostDateDTOs;
using VJN.ModelsDTO.SlotDTOs;

namespace VJN.ModelsDTO.PostJobDTOs
{
    public class PostJobDetailForUpdate
    {
        public int PostId { get; set; }
        public string? JobTitle { get; set; }
        public string? JobDescription { get; set; }
        public int? SalaryTypesId { get; set; }
        public decimal? Salary { get; set; }
        public int? NumberPeople { get; set; }
        public string? Address { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public DateTime? CreateDate { get; set; }
        public int? Status { get; set; }
        public bool? IsUrgentRecruitment { get; set; }
        public int? JobCategoryId { get; set; }
        public List<string>? ImagesURL { get; set; }
        public List<int>? ImagesURLIds { get; set; }
        public IEnumerable<SlotDTO> slotDTOs { get; set; }
        public IEnumerable<JobPostDateDTO> jobPostDateDTOs {  get; set; }
        public bool isLongterm { get; set; }
        public int? Time { get; set; }
    }
}
