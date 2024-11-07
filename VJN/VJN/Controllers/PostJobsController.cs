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
        private readonly VJNDBContext _context;

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
            string userid_str = GetUserIdFromToken();
            int uid = int.Parse(userid_str);

            var id = await _postJobService.CreatePostJob(postJobCreateDTO, uid);
            return Ok(id);
        }

        [Authorize]
        [HttpGet("GetListJobsCreated")]
        public async Task<ActionResult<PagedResult<JobSearchResultEmployer>>> GetListJobsCreated([FromQuery]PostJobSearchEmployer s)
        {
            string userid = GetUserIdFromToken();
            int id = int.Parse(userid);
            var page = await _postJobService.GetJobListByEmployerID(id, s);
            return Ok(page);
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

        [HttpPut("Accept/{id}")]
        public async Task<IActionResult> AcceptPostJob(int id)
        {
            var c = await _postJobService.ChangeStatusPostJob(id, 2);
            if (c)
            {
                return Ok(c);
            }
            else
            {
                return BadRequest(new { Message = "Duyệt bài đăng thất bại" });
            }
        }

        [HttpPut("Reject/{id}")]
        public async Task<IActionResult> RejectPostJob(int id, string reasonRejecr)
        {
            var c = await _postJobService.ChangeStatusPostJob(id, 3);
            //do something with reasonRejecr
            if (c)
            {
                return Ok(c);
            }
            else
            {
                return BadRequest(new { Message = "Từ chối bài đăng thất bại" });
            }
        }

        [HttpPut("Ban/{id}")]
        public async Task<IActionResult> BanPostJob(int id, string reasonBan)
        {
            var c = await _postJobService.ChangeStatusPostJob(id, 6);
            if (c)
            {
                return Ok(c);
            }
            else
            {
                return BadRequest(new { Message = "Cấm bài viết thất bại " });
            }
        }

        [HttpPut("UnBan/{id}")]
        public async Task<IActionResult> RUnBanPostJob(int id)
        {
            var c = await _postJobService.ChangeStatusPostJob(id, 2);
            if (c)
            {
                return Ok(c);
            }
            else
            {
                return BadRequest(new { Message = "Baì viết đã được gỡ lệnh cấm" });
            }
        }

        [HttpGet("GetAllPostJobs")]
        public async Task<ActionResult<PagedResult<PostJobDTOforReport>>> GetAllPostJobs([FromQuery] int status, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {

            IEnumerable<PostJobDTOforReport> postJobs;

            // Lấy dữ liệu từ dịch vụ dựa trên giá trị của status
            if (status == 1)
            {
                // Lấy tất cả các bài đăng với status 1 (yêu cầu duyệt)
                postJobs = await _postJobService.GetAllPostJobByStatus(1);
            }
            else if (status == -1)
            {
                // Lấy tất cả các bài đăng
                postJobs = await _postJobService.GetAllPostJobByStatus(-1);
            }
            //else if (status == 2)
            //{
            //    // Lấy tất cả các bài đăng theo trang thai khac
            //    postJobs = await _postJobService.GetAllPostJobByStatus(stt);
            //}
            else 
            {
                // Lấy các bài đăng có báo cáo (Reports)
                postJobs = (await _postJobService.GetAllPostJobByStatus(-1))
                            .Where(item => item.Reports != null && item.Reports.Any());
            }
            var pagedResult = postJobs.GetPaged(pageNumber, pageSize);

            return Ok(pagedResult);
        }

        [HttpGet("GetPostDetailForStaff")]
        public async Task<IActionResult> GetPostDetailForStaff(int id)
        {
            var post = await _postJobService.GetPostByIDForStaff(id);
            if (post!=null)
            {
                return Ok(post);
            }
            else
            {
                return BadRequest(new { Message = "Bài đăng ko tồn tại" });
            }
        }

    }
}
