using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VJN.Models;
using VJN.ModelsDTO.ApplyJobDTOs;
using VJN.ModelsDTO.UserDTOs;
using VJN.Services;

namespace VJN.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobEmployerController : ControllerBase
    {
        private readonly IApplyJobService _applyJoBService;
        private readonly IPostJobService _postJobService;
        private readonly IUserService _userService;
        private readonly VJNDBContext _context;
        private readonly IMapper _mapper;
        public JobEmployerController(IApplyJobService applyJoBService, IPostJobService postJobService, IUserService userService, VJNDBContext context, IMapper mapper )
        {
            _applyJoBService = applyJoBService;
            _postJobService = postJobService;
            _userService = userService;
            _context = context;
            _mapper = mapper;
        }

        // GET: api/JobEmployer/GetAllJobseekerApply/{post_ID}
        [HttpGet("GetAllJobseekerApply/{post_ID}")]
        public async Task<ActionResult<IEnumerable<UserDTOforList>>> GetAllJobSeekerApplied(int post_ID)
        {
            try
            {
                var JobSeekersApplied = await _applyJoBService.getApplyJobByPostId(post_ID);
                var userdtoforlist = new List <UserDTOforList>();
                foreach (var item in JobSeekersApplied) 
                {
                    var _user = await _userService.findById(item.JobSeekerId ?? 0);
                    userdtoforlist.Add(_mapper.Map<UserDTOforList>(_user));
                }
                return Ok(userdtoforlist);
            }
            catch (Exception ex) 
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }

        }

        // GET: api/JobEmployer/GetDetailJobseekerApply/{JobSeekerApply_ID}
        [HttpGet("GetDetailJobseekerApply/{JobSeekerApply_ID}")]
        public async Task<ActionResult<UserDTOdetail>> GetDetailJobSeekerApply(int JobSeekerApply_ID)
        {
            try
            {
                var jobseeker = await _userService.GetUserDetail(JobSeekerApply_ID);
                return jobseeker;
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: api/JobEmployer/ChangeStatusApplyJob/{JobSeekerApply_ID}
        [HttpGet("ChangeStatusApplyJob")]
        public async Task<bool> ChangeStatusOfJobseekerApply(int JobSeekerApply_ID,int newStatus)
        {
            return await _applyJoBService.ChangeStatusOfJobseekerApply(JobSeekerApply_ID,newStatus);
        }
    }
}
