using VJN.Models;

namespace VJN.ModelsDTO.ReportDTOs
{
    public class ReportDTO
    {
        public int ReportId { get; set; }
        public string? Reason { get; set; }
        public DateTime? CreateDate { get; set; }
        public int? Status { get; set; }
        public  UserDTOs.UserDTOReport? JobSeeker { get; set; }
        public  ICollection<ReportMedium> ReportMedia { get; set; }
    }
}
