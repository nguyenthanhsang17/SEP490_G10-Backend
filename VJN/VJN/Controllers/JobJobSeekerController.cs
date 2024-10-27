using Microsoft.AspNetCore.Hosting.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VJN.Models;
using VJN.ModelsDTO.ApplyJobDTOs;
using VJN.ModelsDTO.PostJobDTOs;
using VJN.ModelsDTO.UserDTOs;
using VJN.Paging;
using VJN.Services;

namespace VJN.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobJobSeekerController : ControllerBase
    {
        private readonly IApplyJobService _applyJoBService ;
        private readonly IPostJobService _postJobService;
        private readonly IUserService _userService;
        private readonly VJNDBContext _context;
        public JobJobSeekerController(IApplyJobService applyJoBService, IPostJobService postJobService, IUserService userService, VJNDBContext context)
        {
            _applyJoBService = applyJoBService;
            _postJobService = postJobService;
            _userService = userService;
            _context = context;
        }
        // GET: api/JobJobSeeker/GetAllJobApplied/{JobSeeker_ID}
        [HttpGet("GetAllJobApplied/{JobSeeker_ID}")]
        public async Task<ActionResult<PagedResult<ApplyJobForListApplied>>> GetAllJobAppliedByUserId(
        int JobSeeker_ID,
        int pageNumber = 1,
        int pageSize = 10)
        {
            try
            {
                var appliedJobs = await _applyJoBService.getApplyJobByJobSeekerId(JobSeeker_ID);
                var appliedJobList = new List<ApplyJobForListApplied>();

                foreach (var appliedJob in appliedJobs)
                {
                    var postJobDTOForList = await _postJobService.GetPostJobById(appliedJob.PostId ?? 0);
                    string authorName = "";
                    int? authorid = postJobDTOForList.AuthorId;
                    if (authorid.HasValue)
                    {
                        UserDTO user = await _userService.findById(authorid.Value);
                        authorName = user.FullName;
                    }

                    var salarytype = await _context.SalaryTypes.FindAsync(postJobDTOForList.SalaryTypesId);
                    string salaryName = salarytype.TypeName;

                    var jobcategory = await _context.JobCategories.FindAsync(postJobDTOForList.JobCategoryId);
                    string jobcategoryname = jobcategory.JobCategoryName;

                    appliedJobList.Add(new ApplyJobForListApplied
                    {
                        Id = appliedJob.Id,
                        JobSeekerId = appliedJob.JobSeekerId,
                        PostId = appliedJob.PostId,
                        JobTitle = postJobDTOForList?.JobTitle,
                        SalaryType = salaryName,
                        RangeSalaryMin = postJobDTOForList?.RangeSalaryMin,
                        RangeSalaryMax = postJobDTOForList?.RangeSalaryMax,
                        FixSalary = postJobDTOForList?.FixSalary,
                        NumberPeople = postJobDTOForList?.NumberPeople,
                        Authorname = authorName,
                        CreateDate = postJobDTOForList?.CreateDate,
                        ExpirationDate = postJobDTOForList?.ExpirationDate,
                        StatusApplyJob = (int)appliedJob.Status,
                        StatusJob = (int)postJobDTOForList.Status,
                        JobCategory = jobcategoryname
                    });
                }

                // Áp dụng phân trang bằng PaginationHelper
                var pagedResult = appliedJobList.GetPaged(pageNumber, pageSize);

                return Ok(pagedResult);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


    }
}
