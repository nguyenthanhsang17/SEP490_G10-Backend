namespace VJN.ModelsDTO.JobPostDateDTOs
{
    public class JobPostDateCreateDTO
    {
        public int? PostId { get; set; }
        public DateTime? EventDate { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
    }
}
