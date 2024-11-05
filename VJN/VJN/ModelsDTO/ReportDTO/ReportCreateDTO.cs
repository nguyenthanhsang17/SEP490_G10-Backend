namespace VJN.ModelsDTO.ReportDTO
{
    public class ReportCreateDTO
    {
        public string? Reason { get; set; }
        public int? PostId { get; set; }
        public List<IFormFile>? files { get; set; }
    }
}
