namespace VJN.ModelsDTO.PostJobDTOs
{
    //miêu tả list post job created cho employer
    public class PostJobCreatedDTO
    {
        public int PostId { get; set; }
        public string? thumbnail { get; set; }
        public string JobTitle { get; set; }
        public decimal? Salary { get; set; }
        public int? NumberPeople { get; set; }
        public string Address { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public double? distance { get; set; }
        public string AuthorName { get; set; }
        public string SalaryTypeName { get; set; }
        public string JobCategoryName { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public int? Status { get; set; }
        public bool? IsUrgentRecruitment { get; set; }
        public int NumberOfApplicants { get; set; }
    }
}
