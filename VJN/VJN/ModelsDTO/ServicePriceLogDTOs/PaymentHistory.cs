using VJN.Models;
using VJN.ModelsDTO.ServicePriceListDTOs;
using VJN.ModelsDTO.UserDTOs;

namespace VJN.ModelsDTO.ServicePriceLogDTOs
{
    public class PaymentHistory
    {
        public int ServicePriceLogId { get; set; }
        public int? UserId { get; set; }
        public int? ServicePriceId { get; set; }
        public DateTime? RegisterDate { get; set; }
        public UserDTOdetail user {  get; set; }
        public ServicePriceListDTO servicePrice {  get; set; }
    }
}
