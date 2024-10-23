namespace VJN.ModelsDTO.PostJobDTOs
{
    public class PostJobSearch
    {
        public string? JobTitle { get; set; }
        public string? JobDescription { get; set; }
        public int? SalaryTypesId { get; set; } // = 0 la chọn tất 
        public decimal? RangeSalaryMin { get; set; }
        public decimal? RangeSalaryMax { get; set; }
        public string? Address { get; set; }
        public decimal? Latitude { get; set; } //ko cân giao dien lay luoon cuar user
        public decimal? Longitude { get; set; }//ko cân giao dien lay luoon cuar user
        public double ? distance { get; set; }
        public bool? IsUrgentRecruitment { get; set; }
        public int? JobCategoryId { get; set; } // = 0 la chọn tất 
        public int? SortNumberApplied {  get; set; } // 0 la ko sort, -1 giam dan, 1 là tang dan
        public int pageNumber { get; set; }// trang muoons xem
    }
}
