namespace VJN.ModelsDTO.PostJobDTOs
{
    public class PostJobSearch
    {
        public string? JobTitle { get; set; }
        public string? JobDescription { get; set; }
        public int? SalaryTypesId { get; set; }
        public decimal? RangeSalaryMin { get; set; }
        public decimal? RangeSalaryMax { get; set; }
        public string? Address { get; set; }
        public decimal? Latitude { get; set; } //ko cân giao dien lay luoon cuar user
        public decimal? Longitude { get; set; }//ko cân giao dien lay luoon cuar user
        public double ? distance { get; set; }
        public bool? IsUrgentRecruitment { get; set; }
        public int? JobCategoryId { get; set; }
    }
}
