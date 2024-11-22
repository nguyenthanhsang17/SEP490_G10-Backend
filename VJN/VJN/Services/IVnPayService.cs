using VJN.ModelsDTO.VnPayDTO;

namespace VJN.Services
{
    public interface IVnPayService
    {
        public string CreatePaymentUrl(HttpContext context, VnPaymentRequestModel model);
        public VnPaymentResponseModel PaymentExecute(IQueryCollection collections);
    }
}
