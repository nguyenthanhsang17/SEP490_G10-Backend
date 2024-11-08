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
using VJN.ModelsDTO.PostJobDTOs;
using VJN.ModelsDTO.WishJob;
using VJN.Paging;
using VJN.Services;

namespace VJN.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WishJobsController : ControllerBase
    {
        private readonly IPostJobService _postJobService;

        public WishJobsController(IPostJobService postJobService)
        {
            _postJobService = postJobService;
        }

        [Authorize]
        [HttpPost("AddWishJob")]
        public async Task<IActionResult> AddWishJob([FromBody] WishJobCreateDTO model)
        {
            if (model == null || model.PostJobId == null)
            {
                return BadRequest(new { Message = "Không có ID công việc" });
            }
            var id_str = GetUserIdFromToken();
            var id = int.Parse(id_str);
            var check = await _postJobService.AddWishJob(model.PostJobId.Value, id);
            if (check)
            {
                return Ok(new {Message="Thêm công việc vào mục yêu thích"});
            }
            else
            {
                return BadRequest(new { Message = "Công việc đã có trong mục yêu thích" });
            }
        }

        [Authorize]
        [HttpDelete("DeleteWishJob")]
        public async Task<IActionResult> DeleteWishJob([FromBody] WishJobCreateDTO model)
        {
            if (model == null || model.PostJobId == null)
            {
                return BadRequest(new { Message = "Không có ID công việc" });
            }
            var id_str = GetUserIdFromToken();
            var id = int.Parse(id_str);
            var check  =await _postJobService.DeleteWishJob(model.PostJobId.Value, id);
            if (check)
            {
                return Ok(new { Message = "Xóa công việc khỏi danh sách mong muốn thành công" });
            }
            else
            {
                return BadRequest(new { Message= "Xóa công việc khỏi danh sách mong muốn thất bại"});
            }

        }


        [Authorize]
        [HttpGet("getAllWishListJob")]
        public async Task<ActionResult<PagedResult<JobSearchResult>>> getAllWishListJob([FromQuery] PostJobSearchWishList model)
        {
            var id_str = GetUserIdFromToken().ToString();
            var userid = int.Parse(id_str);
            var paged = await _postJobService.getJobWishList(model, userid);
            return Ok(paged);
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
