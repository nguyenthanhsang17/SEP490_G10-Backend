using System;
using System.Collections.Generic;
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
            var jobs = await _postJobService.SearchJobPopular(model, pageNumber);
            if(jobs == null || jobs.Items.Count() == 0)
            {
                return BadRequest(new { Message = "không tìm thấy công việc !!!" });
            }
            return Ok(jobs);
        }
    }
}
