using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using VJN.ModelsDTO.ServicePriceLogDTOs;
using VJN.ModelsDTO.VnPayDTO;
using VJN.Services;

namespace VJN.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VnPayController : ControllerBase
    {
        private readonly IVnPayService _vnPayService;
        private readonly IServicePriceLogService _servicePriceLogService;
        private readonly IServicePriceListService _servicePriceListService;
        private IUserService _userService;

        public VnPayController(IVnPayService vnPayService, IServicePriceLogService servicePriceLogService, IServicePriceListService servicePriceListService, IUserService userService)
        {
            _vnPayService = vnPayService;
            _servicePriceLogService = servicePriceLogService;
            _servicePriceListService = servicePriceListService;
            _userService = userService;
        }
        [Authorize]
        [HttpPost("checkout")]
        public async Task<ActionResult<string>> GetVnPay([FromBody] ServicePriceLogForCreateDTO model)
        {
            var id_str = GetUserIdFromToken();
            int userid = 0;
            if (!string.IsNullOrEmpty(id_str))
            {
                userid = int.Parse(id_str);
            }

            try
            {
                var ServicePriceLog = await _servicePriceListService.GetServicePriceById(model.ServicePriceId.Value);
                var user = await _userService.findById(userid);
                var req = new VnPaymentRequestModel()
                {
                    FullName = user.FullName,
                    Description = "Mua gói từ VJN",
                    ServiceId = model.ServicePriceId.Value,
                    ServiceName = ServicePriceLog.ServicePriceId,
                    Amount = ServicePriceLog.Price,
                    CreatedDate = DateTime.Now,
                };

                var url = _vnPayService.CreatePaymentUrl(HttpContext, req);
                return Ok(url);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "An error occurred while processing your request.", details = ex.Message });
            }
        }
        [Authorize]
        [HttpPost("PaymentCallBack/{id}")]
        public async Task<IActionResult> PaymentCallBack(int id)
        {
            var id_str = GetUserIdFromToken();
            int userid = 0;
            if (!string.IsNullOrEmpty(id_str))
            {
                userid = int.Parse(id_str);
            }
            var result = _vnPayService.PaymentExecute(Request.Query);

            if (result == null || result.VnPayResponseCode != "00" || result.Success == false) {
                return BadRequest(new { message = "Giao dịch thất bại"});
            }

            var check =  await _servicePriceLogService.CreateServicePriceLog(id, userid);
            Console.WriteLine(check);
            return Ok(result);

        }

        private string GetUserIdFromToken()
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            Console.WriteLine(token);
            if (string.IsNullOrEmpty(token))
            {
                return null;
            }

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            var userIdClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "nameid");

            if (userIdClaim == null)
            {
                return null;
            }

            return userIdClaim.Value;
        }


    }
}
