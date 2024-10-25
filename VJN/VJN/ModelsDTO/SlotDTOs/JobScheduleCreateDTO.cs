namespace VJN.ModelsDTO.SlotDTOs
{
    public class JobScheduleCreateDTO
    {
        public int? SlotId { get; set; }
        public int? DayOfWeek { get; set; }
        public IEnumerable<WorkingHourCreateDTO>? workingHourCreateDTOs { get; set; }
    }
}
