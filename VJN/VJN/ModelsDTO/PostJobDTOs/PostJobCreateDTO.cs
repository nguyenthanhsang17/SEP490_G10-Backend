namespace VJN.ModelsDTO.PostJobDTOs
{
    public class PostJobCreateDTO
    {
        public string? JobTitle { get; set; }
        public string? JobDescription { get; set; }
        public int? SalaryTypesId { get; set; }
        public decimal? Salary { get; set; }
        public int? NumberPeople { get; set; }
        public string? Address { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public int? AuthorId { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public int? Status { get; set; }
        public int? CensorId { get; set; }
        public DateTime? CensorDate { get; set; }
        public bool? IsUrgentRecruitment { get; set; }
        public int? JobCategoryId { get; set; }
    }
}
