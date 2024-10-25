using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
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
