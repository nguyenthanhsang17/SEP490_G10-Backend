namespace VJN.ModelsDTO.SlotDTOs
{
    public class SlotCreateDTO
    {
        public int? PostId { get; set; }
        public int? UserId { get; set; }

        public IEnumerable<JobScheduleCreateDTO>? jobScheduleCreateDTO { get; set; }
    }
}
