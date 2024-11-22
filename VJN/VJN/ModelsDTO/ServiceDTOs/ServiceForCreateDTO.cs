namespace VJN.ModelsDTO.ServiceDTOs
{
    public class ServiceForCreateDTO
    {
        public int? UserId { get; set; }
        public int? NumberPosts { get; set; }
        public int? NumberPostsUrgentRecruitment { get; set; }
        public int? IsFindJobseekers { get; set; }
        public DateTime? ExpirationDate { get; set; }
    }
}
