namespace VJN.ModelsDTO.PostJobDTOs
{
    public class PostJobDTOForList
    {
        public int PostId { get; set; }
        public string? JobTitle { get; set; }
        public int SalaryTypesId { get; set; }
        public decimal? RangeSalaryMin { get; set; }
        public decimal? RangeSalaryMax { get; set; }
        public decimal? FixSalary { get; set; }
        public int? NumberPeople { get; set; }
        public int? AuthorId { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public int? JobCategoryId { get; set; }
    }
}
