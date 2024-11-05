using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using VJN.Models;
using VJN.ModelsDTO.ApplyJobDTOs;
using VJN.ModelsDTO.FavoriteListDTOs;
using VJN.ModelsDTO.JobSeekerDTOs;
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
        private readonly IApplyJobService _applyJoBService;
        private readonly IPostJobService _postJobService;
        private readonly IUserService _userService;
        private readonly IJobSeekerService _seekerService;
        private readonly VJNDBContext _context;
        public JobJobSeekerController(IApplyJobService applyJoBService, IPostJobService postJobService, IUserService userService, VJNDBContext context, IJobSeekerService jobSeekerService)
        {
            _applyJoBService = applyJoBService;
            _postJobService = postJobService;
            _userService = userService;
            _context = context;
            _seekerService = jobSeekerService;
        }

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
            foreach (var claim in jwtToken.Claims)
            {
                Console.WriteLine($"Claim Type: {claim.Type}, Claim Value: {claim.Value}");
            }
            var userIdClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "nameid");

            if (userIdClaim == null)
            {
                throw new Exception("User ID not found in token.");
            }

            return userIdClaim.Value;
        }


        // GET: api/JobJobSeeker/GetAllJobApplied}
        [HttpGet("GetAllJobApplied")]
        public async Task<ActionResult<PagedResult<ApplyJobForListApplied>>> GetAllJobAppliedByUserId(int pageNumber = 1,int pageSize = 10)
        {
            string userid_str = GetUserIdFromToken();
            int JobSeeker_ID = int.Parse(userid_str);
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
        //[Authorize]
        //[HttpGet]
        //public async Task<PagedResult<JobSeekerDTO>> GetAllJobSeeker()
        //{

        //}

        [Authorize]
        [HttpGet("GetJobSeekerDetail/{id}")]
        public async Task<JobSeekerDetailDTO> GetJobSeekerDetail(int id)
        {
            var dto = await _seekerService.GetJobSeekerByIserID(id);
            return dto;
        }

    }
}
