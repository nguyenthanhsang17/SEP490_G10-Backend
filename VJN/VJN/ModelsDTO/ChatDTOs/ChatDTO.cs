namespace VJN.ModelsDTO.ChatDTOs
{
    public class ChatDTO
    {
        public int ChatId { get; set; }
        public int? SendFromId { get; set; }
        public int? SendToId { get; set; }
        public string? Message { get; set; }
        public DateTime? SendTime { get; set; }
    }
}
