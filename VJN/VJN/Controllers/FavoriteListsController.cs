using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;
using VJN.Models;
using VJN.ModelsDTO.FavoriteListDTOs;
using VJN.ModelsDTO.JobSeekerDTOs;
using VJN.Paging;
using VJN.Services;

namespace VJN.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FavoriteListsController : ControllerBase
    {

        private readonly IJobSeekerService _jobSeekerService;

        public FavoriteListsController(IJobSeekerService jobSeekerService)
        {
            _jobSeekerService = jobSeekerService;
        }

        [Authorize]
        [HttpPost("AddFavorite")]
        public async Task<IActionResult> AddFavorite(FavoriteListCreateDTO model)
        {
            var id_str = GetUserIdFromToken();
            var userid = int.Parse(id_str.ToString());
            var c = await _jobSeekerService.AddFavorite(model, userid);
            if (c)
            {
                return Ok(new { Message = "Thêm thành công" });
            }
            else
            {
                return BadRequest(new { Message = "Thêm Thất bại" });
            }
        }

        [Authorize]
        [HttpDelete("DeleteFavorite/{jobseekerid}")]
        public async Task<IActionResult> DeleteFavorite( int jobseekerid)
        {

            var id_str = GetUserIdFromToken();
            var userid = int.Parse(id_str.ToString());
            var c = await _jobSeekerService.DeleteFavorite(jobseekerid, userid);
            if (c)
            {
                return Ok(new { Message = "Xóa thành công" });
            }
            else
            {
                return BadRequest(new { Message = "Xóa Thất bại" });
            }
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<PagedResult<JobSeekerDTO>>> GetAllFavorite([FromQuery] FavoriteListSearch model)
        {
            var id_str = GetUserIdFromToken();
            var userid = int.Parse(id_str.ToString());
            var paged = await _jobSeekerService.GetAllFavoriteList(model, userid);
            return paged;
        }

        private string GetRoleFromToken()
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            Console.WriteLine(token);
            if (string.IsNullOrEmpty(token))
            {
                throw new Exception("Missing token in Authorization header.");
            }

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var userIdClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "role");

            if (userIdClaim == null)
            {
                throw new Exception("role user not found in token.");
            }

            return userIdClaim.Value;
        }

        private string GetUserIdFromToken()
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            Console.WriteLine(token);
            if (string.IsNullOrEmpty(token))
            {
                throw new Exception("Missing token in Authorization header.");
            }

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var userIdClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "nameid");

            if (userIdClaim == null)
            {
                throw new Exception("User ID not found in token.");
            }

            return userIdClaim.Value;
        }

    }
}
