using VJN.Models;

namespace VJN.ModelsDTO.ServicePriceLogDTOs
{
    public class PaymentHistory
    {
        public int ServicePriceLogId { get; set; }
        public int? UserId { get; set; }
        public int? ServicePriceId { get; set; }
        public DateTime? RegisterDate { get; set; }
    }
}
