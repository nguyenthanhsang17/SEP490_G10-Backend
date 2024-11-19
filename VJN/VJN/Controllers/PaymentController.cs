using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Plugins;
using System.IdentityModel.Tokens.Jwt;
using VJN.ModelsDTO.ServicePriceLogDTOs;
using VJN.Services;

namespace VJN.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IServicePriceLogService _servicePriceLogService;

        public PaymentController(IServicePriceLogService servicePriceLogService)
        {
            _servicePriceLogService = servicePriceLogService;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PaymentHistory>>> GetHistoryPayment() {
            var id_str = GetUserIdFromToken();
            int userid = 0;
            if (!string.IsNullOrEmpty(id_str))
            {
                userid = int.Parse(id_str);
            }
            Console.WriteLine("Userid: " + userid);
            var phe = await _servicePriceLogService.GetPaymentHistory(userid);
            if( phe == null || !phe.Any()|| phe.Count() == 0 )
            {
                return BadRequest(new { Message = "Không có giao dịch" });
            }
            return Ok(phe);
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
            /// check tên claim
            //foreach (var claim in jwtToken.Claims)
            //{
            //    Console.WriteLine($"Claim Type: {claim.Type}, Claim Value: {claim.Value}");
            //}
            /// check tên claim
            var userIdClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "nameid");

            if (userIdClaim == null)
            {
                return null;
            }

            return userIdClaim.Value;
        }
    }

}
