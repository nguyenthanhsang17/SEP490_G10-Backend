using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VJN.Models;
using VJN.ModelsDTO.PostJobDTOs;
using VJN.Paging;
using VJN.Services;

namespace VJN.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostJobsController : ControllerBase
    {
        private readonly IPostJobService _postJobService;

        public PostJobsController(IPostJobService postJobService)
        {
            _postJobService = postJobService;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<JobSearchResult>>> GetPostJobsPopular([FromQuery] PostJobSearch model, [FromQuery] int pageNumber)
        {
            model.JobCategoryId = 0;
            pageNumber = 2;
            model.SalaryTypesId = 0;
            var jobs = await _postJobService.SearchJobPopular(model, pageNumber);
            if(jobs == null || jobs.Items.Count() == 0)
            {
                return BadRequest(new { Message = "không tìm thấy công việc !!!" });
            }
            return Ok(jobs);
        }

        [HttpGet("jobDetails/{id}")]
        public async Task<ActionResult<PostJob>> getJobDetails(int id)
        {
            string iduser_str = GetUserIdFromToken();
            int iduser = int.Parse(iduser_str);



            return null;
        }

        private string GetUserIdFromToken()
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

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
