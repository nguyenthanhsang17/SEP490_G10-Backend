using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VJN.Models;
using VJN.ModelsDTO.ApplyJobDTOs;
using VJN.ModelsDTO.CvDTOs;
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
                    UserDTOforList userDTO = _mapper.Map<UserDTOforList>(_user);
                    userDTO.Apply_id = item.Id;
                    userdtoforlist.Add(userDTO);
                }
                return Ok(userdtoforlist);
            }
            catch (Exception ex) 
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }

        }

        // GET: api/JobEmployer/GetDetailJobseekerApply/{JobSeekerApply_ID}
        [HttpGet("GetDetailJobseekerApply/{JobSeekerApply_ID}/{applyId}")]
        public async Task<ActionResult<object>> GetDetailJobSeekerApply(int JobSeekerApply_ID,int applyId)
        {
            try
            {
                var applyjob = await _context.ApplyJobs.FindAsync(applyId);
                var status = applyjob.Status;
                var jobseeker = await _userService.GetUserDetail(JobSeekerApply_ID);
                var cvs = await _context.Cvs
                .Include(c => c.ItemOfCvs) 
                .Where(c => c.UserId == jobseeker.UserId)
                .ToListAsync();
                jobseeker.Cvs = _mapper.Map<List<CvDTODetail>>(cvs);
                var result = new
                {
                    jobseeker.UserId,
                    jobseeker.Email,
                    jobseeker.AvatarURL,
                    jobseeker.FullName,
                    jobseeker.Age,
                    jobseeker.Phonenumber,
                    jobseeker.CurrentJob,
                    jobseeker.Description,
                    jobseeker.Address,
                    jobseeker.Gender,
                    Cvs = _mapper.Map<List<CvDTODetail>>(cvs),
                    applyID = applyId, // Gán giá trị cho thuộc tính mới
                    status = status,
                };
                return result;
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: api/JobEmployer/ChangeStatusApplyJob
        [HttpGet("ChangeStatusApplyJob")]
        public async Task<bool> ChangeStatusOfJobseekerApply(int Applyjob_Id,int newStatus)
        {
            return await _applyJoBService.ChangeStatusOfJobseekerApply(Applyjob_Id, newStatus);
        }
    }
}
