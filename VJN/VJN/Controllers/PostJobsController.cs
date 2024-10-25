using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Plugins;
using VJN.Models;
using VJN.ModelsDTO.PostJobDTOs;
using VJN.Paging;
using VJN.Repositories;
using VJN.Services;

namespace VJN.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostJobsController : ControllerBase
    {
        private readonly IPostJobService _postJobService;
        private readonly ISlotService _slotService;

        public PostJobsController(IPostJobService postJobService, ISlotService slotService)
        {
            _postJobService = postJobService;
            _slotService = slotService;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<JobSearchResult>>> GetPostJobsPopular([FromQuery] PostJobSearch model)
        {
            var jobs = await _postJobService.SearchJobPopular(model);
            if(jobs == null || jobs.Items.Count() == 0)
            {
                return BadRequest(new { Message = "không tìm thấy công việc !!!" });
            }
            return Ok(jobs);
        }

        [HttpGet("jobDetails/{id}")]
        public async Task<ActionResult<PostJob>> getJobDetails(int id)
        {
            string? iduser_str = GetUserIdFromToken();
            int? iduser = null;
            if (!string.IsNullOrEmpty(iduser_str))
            {
                iduser = int.Parse(iduser_str);
            }

            var postdto = await _postJobService.getJostJobByID(id, iduser);

            var slotDTO = await _slotService.GetSlotByPostjobId(id);

            postdto.slotDTOs = slotDTO;
            return Ok(postdto);
        }
        [HttpPut("ShowPostJob/{id}")]
        public async Task<IActionResult> ShowPostJob(int id)
        {
            var c = await _postJobService.ChangeStatusPostJob(id, 2);
            if (c)
            {
                return Ok(c);
            }
            else
            {
                return BadRequest(new { Message = "Hiện bài post thất bại" });
            }
        }

        [HttpPut("HidePostJob/{id}")]
        public async Task<IActionResult> HidePostJob(int id)
        {
            var c = await _postJobService.ChangeStatusPostJob(id, 5);
            if (c)
            {
                return Ok(c);
            }
            else
            {
                return BadRequest(new { Message = "Ẩn bài post thất bại" });
            }
        }
        [Authorize]
        [HttpPost("CreatePost")]
        public async Task<IActionResult> CreatePostJob([FromBody] PostJobCreateDTO postJobCreateDTO )
        {
            var id = await _postJobService.CreatePostJob(postJobCreateDTO);
            return Ok(id);
        }

        private string GetUserIdFromToken()
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

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
