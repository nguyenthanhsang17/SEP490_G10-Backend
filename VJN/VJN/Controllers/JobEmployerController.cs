using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VJN.Models;
using VJN.ModelsDTO.ApplyJobDTOs;
using VJN.ModelsDTO.CvDTOs;
using VJN.ModelsDTO.UserDTOs;
using VJN.Paging;
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
        public async Task<ActionResult<PagedResult<UserDTOforList>>> GetAllJobSeekerApplied(int post_ID, int pageNumber = 1, int pageSize = 4, string? gender = null, int? age = null, string? jobName = null, int? applyStatus = null)
        {
            try
            {

                var jobSeekersApplied = await _applyJoBService.getApplyJobByPostId(post_ID);
                var userdtoforlist = new List<UserDTOforList>();

                foreach (var item in jobSeekersApplied)
                {
                    var _user = await _userService.findById(item.JobSeekerId ?? 0);
                    if (_user != null)
                    {
                        UserDTOforList userDTO = _mapper.Map<UserDTOforList>(_user);
                        userDTO.Apply_id = item.Id;
                        userdtoforlist.Add(userDTO);
                    }
                }

                if (!string.IsNullOrEmpty(gender))
                {
                    bool isMale = gender.Equals("Nam", StringComparison.OrdinalIgnoreCase);
                    userdtoforlist = userdtoforlist.Where(u => u.Gender == isMale).ToList();
                }

                if (age.HasValue)
                {
                    userdtoforlist = userdtoforlist.Where(u => u.Age == age.Value).ToList();
                }

                if (!string.IsNullOrEmpty(jobName))
                {
                    userdtoforlist = userdtoforlist.Where(u => u.JobName.Equals(jobName, StringComparison.OrdinalIgnoreCase)).ToList();
                }

                if (applyStatus.HasValue)
                {
                    userdtoforlist = userdtoforlist.Where(u => u.Apply_id == applyStatus.Value).ToList();
                }


                var pagedResult = userdtoforlist.AsQueryable().GetPaged(pageNumber, pageSize);

                return Ok(pagedResult);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi hệ thống: {ex.Message}");
            }
        }





        // GET: api/JobEmployer/GetDetailJobseekerApply/{JobSeekerApply_ID}
        [HttpGet("GetDetailJobseekerApply/{JobSeekerApply_ID}/{applyId}")]
        public async Task<ActionResult<object>> GetDetailJobSeekerApply(int JobSeekerApply_ID, int applyId)
        {
            try
            {
                var applyjob = await _context.ApplyJobs.FindAsync(applyId);
                if (applyjob == null) return NotFound("Application not found.");

                var status = applyjob.Status;
                var jobseeker = await _userService.GetUserDetail(JobSeekerApply_ID);

                // Lấy danh sách CV của job seeker
                var cvs = await _context.Cvs
                    .Include(c => c.ItemOfCvs)
                    .ToListAsync();

                // Lọc CV chỉ lấy CV có ID bằng applyjob.CvId
                var cvsfilter = cvs
                    .Where(c => c.CvId == applyjob.CvId)
                    .Select(c => _mapper.Map<CvDTODetail>(c))
                    .ToList();

                jobseeker.Cvs = cvsfilter;

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
                    Cvs = cvsfilter, // Trả về danh sách CV đã được lọc
                    applyID = applyId, // Gán giá trị cho thuộc tính mới
                    status = status,
                };

                return Ok(result); // Trả về kết quả với mã 200
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
