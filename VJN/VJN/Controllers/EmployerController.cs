using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using VJN.ModelsDTO.EmployerDTOs;
using VJN.Services;

namespace VJN.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployerController : ControllerBase
    {
        public readonly IEmployerService _employerService;

        public EmployerController(IEmployerService employerService)
        {
            _employerService = employerService;
        }

        [HttpGet("ViewEmployerProfile")]
        public async Task<ActionResult<EmployerDTO>> ViewEmployerProfile([FromQuery] EmployerCallAPI callAPI)
        {
            var id_str = GetUserIdFromToken();
            int userid = 0;
            if (!string.IsNullOrEmpty(id_str))
            {
                userid = int.Parse(id_str);
            }
            var employer = await _employerService.GetEmployerByUserId(callAPI.Authorid.Value, userid, callAPI.Latitude, callAPI.Longitude);
            return Ok(employer);
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
