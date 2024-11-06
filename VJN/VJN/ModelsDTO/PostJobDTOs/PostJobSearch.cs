namespace VJN.ModelsDTO.PostJobDTOs
{
    public class PostJobSearch
    {
        public string? JobKeyWord { get; set; }
        public int? SalaryTypesId { get; set; } // = 0 la chọn tất 
        public string? Address { get; set; }
        public decimal? Latitude { get; set; } //ko cân giao dien lay luoon cuar user
        public decimal? Longitude { get; set; }//ko cân giao dien lay luoon cuar user
        public double ? distance { get; set; }
        public int? JobCategoryId { get; set; } // = 0 la chọn tất 
        public int? SortNumberApplied {  get; set; } // 0 la ko sort, -1 giam dan, 1 là tang dan
        public int pageNumber { get; set; }// trang muoons xem
    }
}
