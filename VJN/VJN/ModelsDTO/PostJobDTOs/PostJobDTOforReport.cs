using VJN.Models;

namespace VJN.ModelsDTO.PostJobDTOs
{
    public class PostJobDTOforReport
    {

        public int PostId { get; set; }
        public string? JobTitle { get; set; }
        public string? JobDescription { get; set; }
        public string SalaryTypeName { get; set; }
        public decimal? Salary { get; set; }
        public int? NumberPeople { get; set; }
        public string? Address { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public string AuthorName { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public int? Status { get; set; }
        public int? CensorId { get; set; }
        public DateTime? CensorDate { get; set; }
        public bool? IsUrgentRecruitment { get; set; }
        public string JobCategoryName { get; set; }

        public virtual User? Author { get; set; }
        public virtual User? Censor { get; set; }
        public virtual SalaryType? SalaryTypes { get; set; }
        public virtual ICollection<ImagePostJob> ImagePostJobs { get; set; }
        public virtual ICollection<JobPostDate> JobPostDates { get; set; }
        public List<ReportDTO.ReportDTO> Reports { get; set; }

    }
}
