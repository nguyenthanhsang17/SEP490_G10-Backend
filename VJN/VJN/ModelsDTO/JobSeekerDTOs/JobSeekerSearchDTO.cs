namespace VJN.ModelsDTO.JobSeekerDTOs
{
    public class JobSeekerSearchDTO
    {
        public string? keyword {  get; set; }// tìm kiếm cv // null là ko tìm j
        public int? sort {  get; set; }// 0 laf ko sort va 1 laf sort  sắp xếp cho theo số lượt applied của jobbseeker, input radio
        public int? CurrentJob {  get; set; } // 0 là tìm kiếm tất cả // còn lại thì xem database,  select option 
        public int? numberPage { get; set; } //phân trang

        public int? agemin { get; set; } // tìm kiếm tuổi
        public int? agemax { get; set; } // tìm kiếm tuổi
        public string? address { get; set; } // tìm kiếm đia chỉ // null là ko tìm j
        public int? gender { get; set; } // tìm kiếm giới tính -1 là tìm tất cả 0 là nữ 1 là nam select option 

    }
}
