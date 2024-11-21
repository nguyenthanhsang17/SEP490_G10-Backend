namespace VJN.ModelsDTO.BlogDTOs
{
    public class BlogUpdateDTO
    {
        public string? BlogTitle { get; set; }
        public string? BlogDescription { get; set; }
        public IFormFile? Thumbnail { get; set; }
    }
}
