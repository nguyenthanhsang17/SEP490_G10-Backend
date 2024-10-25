namespace VJN.ModelsDTO.PostJobDTOs
{
    public class PostJobSearchEmployer
    {
        public string? JobKeyWord { get; set; }
        public int? SalaryTypesId { get; set; } // = 0 la chọn tất 
        public decimal? RangeSalaryMin { get; set; }
        public decimal? RangeSalaryMax { get; set; }
        public int? Status { get; set; } // = -1 la chọn tất 
        public int? IsUrgentRecruitment { get; set; } // = -1 là chọn tất
        public int? JobCategoryId { get; set; } // = 0 la chọn tất 
        public int? SortNumberApplied { get; set; } // 0 la ko sort, -1 giam dan, 1 là tang dan
        public int pageNumber { get; set; }// trang muoons xem
    }
}
