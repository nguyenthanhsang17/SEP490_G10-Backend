namespace VJN.ModelsDTO.PostJobDTOs
{
    public class PostJobDTOForHomepage
    {
        public int PostId { get; set; }
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
        public bool? IsUrgentRecruitment { get; set; }
        public int? JobCategoryId { get; set; }
        //-----------------------------------------------------
        public string? AuthorName {  get; set; }
        public string? JobCategoryName { get; set; }
        public string? SalaryTypeName { get; set; }
        //------------------------------------------------------
        public string? thumnail {  get; set; }

    }
}
