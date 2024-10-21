using VJN.Models;

namespace VJN.ModelsDTO.JobPostDateDTOs
{
    public class JobPostDateDTO
    {
        public int EventDateId { get; set; }
        public int? PostId { get; set; }
        public DateTime? EventDate { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
    }
}
