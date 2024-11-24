using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VJN.Models;
using VJN.ModelsDTO.ApplyJobDTOs;
using VJN.Services;

namespace VJN.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplyJobsController : ControllerBase
    {
        private readonly IApplyJobService _jobService;

        public ApplyJobsController(IApplyJobService jobService)
        {
            _jobService = jobService;
        }

        [Authorize]
        [HttpPut("CancelApplyJob")]
        public async Task<IActionResult> CancelApplyJob(int postjob)
        {
            string  userid_str = GetUserIdFromToken();
            var c = await _jobService.CancelApplyJob(postjob, int.Parse(userid_str));
            return c?Ok(c):BadRequest(c);
        }

        [Authorize]
        [HttpPost("ApplyJob")]
        public async Task<IActionResult> ApplyJob([FromBody] ApplyJobCreateDTO applyJobCreateDTO)
        {
            string userid_str = GetUserIdFromToken();
            var uid= int.Parse(userid_str);
            var c = await _jobService.ApplyJob(applyJobCreateDTO, uid);
            return c ? Ok(c) : BadRequest(c);
        }

        [Authorize]
        [HttpGet("GetApplied")]
        public async Task<IActionResult> GetApplied( int jobid)
        {
            try
            {
                string userid_str = GetUserIdFromToken();
                var uid = int.Parse(userid_str);
                var appliedJobs = await _jobService.GetApplyJobsByUserIdAndPostId(uid, jobid);
                if (appliedJobs != null && appliedJobs.Any())
                {
                    return Ok(appliedJobs);
                }
                else
                {
                    return NotFound("Không tìm thấy ứng tuyển phù hợp."); 
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Đã xảy ra lỗi trong quá trình xử lý.");
            }
        }

        [Authorize]
        [HttpPost("ReApplyJob")]
        public async Task<IActionResult> ReApplyJob([FromBody] ApplyJobCreateDTO applyJobCreateDTO)
        {
            int? rs = -1;
            string userid_str = GetUserIdFromToken();
            var uid = int.Parse(userid_str);
            var appliedJobs = await _jobService.GetApplyJobsByUserIdAndPostId(uid,(int) applyJobCreateDTO.PostId);
            if (appliedJobs != null && appliedJobs.Any())
            {
                foreach (var item in appliedJobs)
                {
                    if (item.Status == 1)
                    {
                        rs = item.PostId;
                        var c = await _jobService.ApplyJob(applyJobCreateDTO, uid);
                        return c ? Ok(c) : BadRequest(c);
                    }
                    if (item.Status == 0)
                    {
                        rs = item.PostId;
                        var c = await _jobService.ReApplyJob(item.Id,(int) applyJobCreateDTO.CvId);
                    }
                }
                return Ok(rs);
            }
            return  BadRequest("Ứng tuyển lại thất bại");
        }


        //ham lay dc userid dua vao token
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
