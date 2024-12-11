using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Plugins;
using System.IdentityModel.Tokens.Jwt;
using VJN.ModelsDTO.ServicePriceLogDTOs;
using VJN.Paging;
using VJN.Services;

namespace VJN.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IServicePriceLogService _servicePriceLogService;
        private readonly IUserService _userService;
        private readonly IServicePriceListService _servicePriceListService;
        public PaymentController(IServicePriceLogService servicePriceLogService, IUserService userService, IServicePriceListService servicePriceListService)
        {
            _servicePriceLogService = servicePriceLogService;
            _userService = userService;
            _servicePriceListService = servicePriceListService;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PaymentHistory>>> GetHistoryPayment(int pageNumber = 1, int pageSize = 5)
        {
            var id_str = GetUserIdFromToken();
            int userid = 0;
            if (!string.IsNullOrEmpty(id_str))
            {
                userid = int.Parse(id_str);
            }
            Console.WriteLine("Userid: " + userid);

            var phe = await _servicePriceLogService.GetPaymentHistory(userid);

            if (phe == null || !phe.Any() || phe.Count() == 0)
            {
                return BadRequest(new { Message = "Không có giao dịch" });
            }

            phe = phe.OrderByDescending(x => x.RegisterDate).ToList();

            var totalCount = phe.Count(); 
            var pagedData = phe.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            foreach (var item in pagedData)
            {
                int prid = (int)item.ServicePriceId;
                item.servicePrice = await _servicePriceListService.GetServicePriceById(prid);

                int useid = (int)item.UserId;
                item.user = await _userService.GetUserDetail(useid);
            }
            return Ok(new
            {
                items = pagedData,
                totalCount = totalCount
            });
        }


        [HttpGet("GetallHistoryPayment")]
        public async Task<ActionResult<PagedResult<PaymentHistory>>> GetAllHistoryPayment(int? uid, [FromQuery] string? username="",
        int pageNumber = 1,int pageSize = 10,int daysFilter = 0, // 0: Lấy tất cả
        int? servicePriceId = null) 
        {

            var phe = await _servicePriceLogService.GetAllPaymentHistory();
            if (phe == null || !phe.Any())
            {
                return BadRequest(new { Message = "Không có giao dịch" });
            }


            if (daysFilter > 0)
            {
                var filterDate = DateTime.UtcNow.AddDays(-daysFilter);
                phe = phe.Where(x => x.RegisterDate >= filterDate).ToList();
            }
            if (servicePriceId.HasValue)
            {
                phe = phe.Where(x => x.ServicePriceId == servicePriceId.Value).ToList();
            }

            if (!phe.Any())
            {
                return BadRequest(new { Message = "Không có giao dịch khớp với điều kiện" });
            }


            foreach (var item in phe)
            {
                int prid = (int)item.ServicePriceId;
                item.servicePrice = await _servicePriceListService.GetServicePriceById(prid);
                int useid = (int)item.UserId;
                item.user = await _userService.GetUserDetail(useid);
            }
            Console.WriteLine(phe.Count());

            if (!string.IsNullOrWhiteSpace(username))
            {
                phe = phe.Where(x => x.user.FullName.Contains(username, StringComparison.OrdinalIgnoreCase)).ToList();
            }
            Console.WriteLine(phe.Count());

            phe = phe.OrderByDescending(x => x.RegisterDate).ToList();
            if (uid.HasValue)
            {
                phe=phe.Where(p=>p.UserId== uid.Value).ToList();
            }
            // Phân trang
            var pagedResult = phe.GetPaged(pageNumber, pageSize);
            return Ok(pagedResult);
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
