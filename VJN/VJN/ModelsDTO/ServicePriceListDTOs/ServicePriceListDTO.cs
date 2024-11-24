namespace VJN.ModelsDTO.ServicePriceListDTOs
{
    public class ServicePriceListDTO
    {
        public int ServicePriceId { get; set; }
        public int? NumberPosts { get; set; }
        public int? NumberPostsUrgentRecruitment { get; set; }
        public int? IsFindJobseekers { get; set; }
        public int? DurationsMonth { get; set; }
        public decimal? Price { get; set; }
        public string? ServicePriceName { get; set; }
        public int? Status { get; set; }
    }
}
