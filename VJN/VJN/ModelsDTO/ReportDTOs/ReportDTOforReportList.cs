using VJN.ModelsDTO.UserDTOs;

namespace VJN.ModelsDTO.ReportDTOs
{
    public class ReportDTOforReportList
    {
        public int ReportId { get; set; }
        public string? Reason { get; set; }
        public DateTime? CreateDate { get; set; }
        public int? Status { get; set; }

        public UserDTOReport JobSeeker { get; set; }
        public virtual ICollection<ReportMediaDTO> ReportMedia { get; set; }
    }
}
