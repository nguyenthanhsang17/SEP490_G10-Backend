namespace VJN.ModelsDTO.SlotDTOs
{
    public class WorkingHourDTO
    {
        public int WorkingHourId { get; set; }
        public int? ScheduleId { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
    }
}
