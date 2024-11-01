using VJN.Models;

namespace VJN.ModelsDTO.ReportDTO
{
    public class ReportDTO
    {
        public int ReportId { get; set; }
        public string? Reason { get; set; }
        public DateTime? CreateDate { get; set; }
        public int? Status { get; set; }

        public string jobseekerName { get; set; }
        public virtual ICollection<ReportMedium> ReportMedia { get; set; }
    }
}
