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
    }
}
