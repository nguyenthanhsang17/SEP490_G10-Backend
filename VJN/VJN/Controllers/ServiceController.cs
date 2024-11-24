using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using VJN.ModelsDTO.ServiceDTOs;
using VJN.Services;

namespace VJN.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceController : ControllerBase
    {
        private readonly IServicePriceLogService _servicePriceLogService;

        public ServiceController(IServicePriceLogService servicePriceLogService)
        {
            _servicePriceLogService = servicePriceLogService;
        }
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<ServiceDTO>> GetService() {
            var userid_str = GetUserIdFromToken();
            var userid = int.Parse(userid_str);
            var sv = await  _servicePriceLogService.GetAllServiceByUserId(userid);
            return Ok(sv);
        }

        private string GetUserIdFromToken()
        {
            try
            {
                // Lấy header Authorization
                if (!HttpContext.Request.Headers.TryGetValue("Authorization", out var authorizationHeader))
                {
                    return null; // Không có header Authorization
                }

                // Loại bỏ "Bearer " nếu có
                var token = authorizationHeader.ToString().Replace("Bearer ", "").Trim();
                if (string.IsNullOrEmpty(token))
                {
                    return null; // Token rỗng
                }

                // Tạo handler và kiểm tra token
                var handler = new JwtSecurityTokenHandler();
                if (!handler.CanReadToken(token))
                {
                    return null; // Token không đọc được
                }

                // Đọc token và lấy claim "nameid"
                var jwtToken = handler.ReadJwtToken(token);
                var userIdClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "nameid");

                return userIdClaim?.Value; // Trả về giá trị hoặc null nếu không có claim
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in GetUserIdFromToken: " + ex.Message);
                return null; // Xử lý lỗi và trả về null
            }
        }
    }
}
