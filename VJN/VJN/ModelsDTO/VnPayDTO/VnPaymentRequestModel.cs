namespace VJN.ModelsDTO.VnPayDTO
{
    public class VnPaymentRequestModel
    {
        public string? FullName { get; set; }
        public string? Description { get; set; }
        public int? ServiceId {  get; set; }
        public int? ServiceName { get; set; }
        public decimal? Amount { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
