using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
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
        private readonly IEmailService _emailService;
        private readonly VJNDBContext _context;

        public ApplyJobsController(IApplyJobService jobService, IEmailService emailService, VJNDBContext context)
        {
            _jobService = jobService;
            _emailService = emailService;
            _context = context;
        }

        [Authorize]
        [HttpPut("CancelApplyJob")]
        public async Task<IActionResult> CancelApplyJob(int postjob)
        {
            string userid_str = GetUserIdFromToken();
            var c = await _jobService.CancelApplyJob(postjob, int.Parse(userid_str));
            return c ? Ok(c) : BadRequest(c);
        }

        [Authorize]
        [HttpPost("ApplyJob")]
        public async Task<IActionResult> ApplyJob([FromBody] ApplyJobCreateDTO applyJobCreateDTO)
        {

            string userid_str = GetUserIdFromToken();
            var uid = int.Parse(userid_str);
            var c = await _jobService.ApplyJob(applyJobCreateDTO, uid);

            var user = await _context.Users.Where(u => u.UserId == uid).SingleOrDefaultAsync();
            var postjob = await _context.PostJobs.Where(p => p.PostId == applyJobCreateDTO.PostId).SingleOrDefaultAsync();
            if (c)
            {
                var html = _emailService.GetEmailHTML("Bạn đã ứng tuyển công việc thành công", "Chào" + user.FullName, $"Chi tiết bài đăng,  Tiêu đề: {postjob.JobTitle}, Mô tả: {postjob.JobDescription} Cảm ơn bạn đã sử dụng dịch vụ của chúng tôi. Trân trọng, Đội ngũ hỗ trợ");

                await _emailService.SendEmailAsyncWithHTML(user.Email, "Bạn đã ứng tuyển công việc thành công", html);
            }
            return c ? Ok(c) : BadRequest(c);
        }

        [HttpGet("CheckPostJob")]
        public async Task<IActionResult> CheckPostJob([FromQuery] int postid)
        {
            var hung = await _context.PostJobs.Where(p => p.PostId == postid).SingleOrDefaultAsync();

            if (hung.Status == 6)
            {
                return Ok(6);
            }
            if (hung.ExpirationDate.Value< DateTime.Now)
            {
                return Ok(7);
            }
            return Ok(1);
        }

        [Authorize]
        [HttpGet("GetApplied")]
        public async Task<IActionResult> GetApplied(int jobid)
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
            var appliedJobs = await _jobService.GetApplyJobsByUserIdAndPostId(uid, (int)applyJobCreateDTO.PostId);
            var c1 = await _context.ApplyJobs.Where(ap => ap.JobSeekerId == uid && ap.PostId == applyJobCreateDTO.PostId).OrderByDescending(ap => ap.ApplyDate).FirstOrDefaultAsync();

            if (appliedJobs != null && appliedJobs.Any())
            {

                if (c1.Status == 1)
                {
                    rs = c1.PostId;
                    var c = await _jobService.ApplyJob(applyJobCreateDTO, uid);
                    return c ? Ok(c) : BadRequest(c);
                }
                if (c1.Status == 0)
                {
                    rs = c1.PostId;
                    var c = await _jobService.ReApplyJob(c1.Id, (int)applyJobCreateDTO.CvId);
                }


                var user = await _context.Users.Where(u => u.UserId == uid).SingleOrDefaultAsync();
                var postjob = await _context.PostJobs.Where(p => p.PostId == applyJobCreateDTO.PostId).SingleOrDefaultAsync();


                var html = _emailService.GetEmailHTML("Bạn đã ứng tuyển lại công việc thành công", "Chào" + user.FullName, $"Chi tiết bài đăng,  Tiêu đề: {postjob.JobTitle}, Mô tả: {postjob.JobDescription} Cảm ơn bạn đã sử dụng dịch vụ của chúng tôi. Trân trọng, Đội ngũ hỗ trợ");

                await _emailService.SendEmailAsyncWithHTML(user.Email, "Bạn đã ứng tuyển lại công việc thành công", html);


                return Ok(rs);
            }
            return BadRequest("Ứng tuyển lại thất bại");
        }
        [HttpGet("Sangtestcode")]
        public async Task<IActionResult> Sangtestcode()
        {
            int i1 = 19;
            int i2 = 3;
            var c = await _context.ApplyJobs.Where(ap => ap.JobSeekerId == i1 && ap.PostId == 3).OrderByDescending(ap => ap.ApplyDate).FirstOrDefaultAsync();
            return Ok(c.Status);
        }

        [Authorize]
        [HttpGet("checkReapply")]
        public async Task<ActionResult<bool>> checkReapply([FromQuery] int postId)
        {
            string userid_str = GetUserIdFromToken();
            var uid = int.Parse(userid_str);
            var a = await _jobService.checkReapply(uid, postId);
            return Ok(a);
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
