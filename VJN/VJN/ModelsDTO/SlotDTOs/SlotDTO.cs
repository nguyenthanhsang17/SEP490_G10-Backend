namespace VJN.ModelsDTO.SlotDTOs
{
    public class SlotDTO
    {
        public int SlotId { get; set; }
        public int? PostId { get; set; }
        public int? UserId { get; set; }
        public IEnumerable<JobScheduleDTO> jobScheduleDTOs { get; set; }
    }
}
