namespace VJN.ModelsDTO.BlogDTOs
{
    public class BlogDTO
    {
        public int BlogId { get; set; }
        public string? BlogTitle { get; set; }
        public string? BlogDescription { get; set; }
        public DateTime? CreateDate { get; set; }
        public int? Status { get; set; }
        public int? AuthorId { get; set; }
        public string? AuthorName { get; set; }
        public string? Thumbnail { get; set; }
    }
}
