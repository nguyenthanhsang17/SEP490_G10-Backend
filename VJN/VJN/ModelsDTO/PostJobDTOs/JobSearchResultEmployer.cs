﻿namespace VJN.ModelsDTO.PostJobDTOs
{
    public class JobSearchResultEmployer
    {
        public int PostId { get; set; }
        public string? thumbnail { get; set; }
        public string JobTitle { get; set; }
        public decimal? Salary { get; set; }
        public int? NumberPeople { get; set; }
        public string Address { get; set; }
        public int? Status { get; set; } // = -1 la chọn tất 
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public DateTime? CreateDate { get; set; }
        public string SalaryTypeName { get; set; }
        public string JobCategoryName { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public bool? IsUrgentRecruitment { get; set; }
        public int NumberOfApplicants { get; set; }
    }
}
