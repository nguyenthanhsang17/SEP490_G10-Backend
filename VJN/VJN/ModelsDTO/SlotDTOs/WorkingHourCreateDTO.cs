namespace VJN.ModelsDTO.SlotDTOs
{
    public class WorkingHourCreateDTO
    {
        public int? ScheduleId { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
    }
}
