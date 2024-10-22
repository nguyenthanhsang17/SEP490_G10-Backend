namespace VJN.ModelsDTO.ApplyJobDTOs
{
    public class ApplyJobForListApplied
    {
        public int Id { get; set; }
        public int? PostId { get; set; }
        public string? JobTitle { get; set; }
        public string? SalaryType { get; set; }
        public decimal? RangeSalaryMin { get; set; }
        public decimal? RangeSalaryMax { get; set; }
        public decimal? FixSalary { get; set; }
        public int? NumberPeople { get; set; }
        public string? Authorname { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public int Status { get; set; }
        public string JobCategory { get; set; }
    }
}
