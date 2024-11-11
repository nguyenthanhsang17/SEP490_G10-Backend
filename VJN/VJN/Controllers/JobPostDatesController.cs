using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VJN.Models;
using VJN.ModelsDTO.JobPostDateDTOs;
using VJN.Services;

namespace VJN.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobPostDatesController : ControllerBase
    {
        private readonly IJobPostDateService _jobPostDateService;

        public JobPostDatesController(IJobPostDateService jobPostDateService)
        {
            _jobPostDateService = jobPostDateService;
        }

        [HttpPost]
        public async Task<ActionResult<bool>> PostJobPostDate(IEnumerable<JobPostDateCreateDTO> jobPostDates)
        {
            var c = _jobPostDateService.CreateJobPostDate(jobPostDates);
            return Ok(c);
        }

        [HttpDelete("DeleteAllJobPostDate/{postid}")]
        public async Task<ActionResult<bool>> DeleteAllJobPostDate(int postid)
        {
           var c =await _jobPostDateService.DeleteAllJobPostDate(postid);
           return c?Ok(c):BadRequest(c);
        }

        [HttpPut("UpdateJobPostDate/{postid}")]
        public async Task<ActionResult<bool>> UpdateJobPostDate(int postid, [FromBody] IEnumerable<JobPostDateForUpdateDTO> jobPostDate)
        {
            var c = await _jobPostDateService.UpdateJobPostDate(postid, jobPostDate);
            return c?Ok(c):BadRequest(c);
        }
    }
}
