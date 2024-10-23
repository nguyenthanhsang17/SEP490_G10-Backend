namespace VJN.ModelsDTO.SlotDTOs
{
    public class JobScheduleDTO
    {
        public int ScheduleId { get; set; }
        public int? SlotId { get; set; }
        public int? DayOfWeek { get; set; }

        public IEnumerable<WorkingHourDTO> workingHourDTOs { get; set; }
    }
}
