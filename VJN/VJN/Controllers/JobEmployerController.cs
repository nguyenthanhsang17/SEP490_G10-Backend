using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
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
        private readonly IEmailService _emailService;
        public JobEmployerController(IApplyJobService applyJoBService, IPostJobService postJobService, IUserService userService, VJNDBContext context, IMapper mapper, IEmailService emailService)

        {
            _applyJoBService = applyJoBService;
            _postJobService = postJobService;
            _userService = userService;
            _context = context;
            _mapper = mapper;
            _emailService = emailService;
        }

        // GET: api/JobEmployer/GetAllJobseekerApply/{post_ID}
        [HttpGet("GetAllJobseekerApply/{post_ID}")]
        public async Task<ActionResult<PagedResult<UserDTOforList>>> GetAllJobSeekerApplied(int post_ID, int pageNumber = 1, int pageSize = 4, string? gender = null, int? agemin = null, int? agemax = null, string? jobName = null, int? applyStatus = null)
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
                        var app = await _context.ApplyJobs.FindAsync(userDTO.Apply_id);
                        userDTO.ApplyStatus =(int) app.Status;
                        userdtoforlist.Add(userDTO);
                    }
                }

                if (!string.IsNullOrEmpty(gender))
                {
                    bool isMale = gender.Equals("Nam", StringComparison.OrdinalIgnoreCase);
                    userdtoforlist = userdtoforlist.Where(u => u.Gender == isMale).ToList();
                }

                if (agemin.HasValue)
                {
                    userdtoforlist = userdtoforlist.Where(u => u.Age >= agemin.Value).ToList();
                }

                if (agemax.HasValue)
                {
                    userdtoforlist = userdtoforlist.Where(u => u.Age <= agemax.Value).ToList();
                }

                if (!string.IsNullOrEmpty(jobName))
                {
                    userdtoforlist = userdtoforlist.Where(u => u.JobName.Equals(jobName, StringComparison.OrdinalIgnoreCase)).ToList();
                }

                if (applyStatus.HasValue)
                {
                    for (int i = userdtoforlist.Count - 1; i >= 0; i--)
                    {
                        var item = userdtoforlist[i];
                        var ap = await _context.ApplyJobs.FindAsync(item.Apply_id);
                        if (ap.Status != applyStatus.Value)
                        {
                            userdtoforlist.RemoveAt(i);
                        }
                    }
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
            var userid_str = GetUserIdFromToken();
            int id = int.Parse(userid_str);
            bool check = await _context.FavoriteLists.Where(ap=>ap.JobSeekerId== JobSeekerApply_ID&&ap.EmployerId==id).AnyAsync();

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
                    Cvs = cvsfilter, 
                    applyID = applyId, 
                    status = status,
                    isFavorite = check==true?1:0,
                };

                return Ok(result); 
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
            var apply = await _context.ApplyJobs.FindAsync(Applyjob_Id);

            var user = await _context.Users.FindAsync(apply.JobSeekerId);
            var postJob = await _context.PostJobs.FindAsync(apply.PostId);
            string stt = "";
            if (newStatus == 1)
            {
                string body = 

                "Chi tiết công việc đã ứng tuyển:\n" +
                $"Tiêu đề: {postJob.JobTitle}\n" +
                $"Mô tả: {postJob.JobDescription}\n" +
                "Đừng buồn hãy thử ứng tuyển bằng cv khác.\n\n" +
                "Chúc bạn sớm kiếm được công việc mong muốn.\n\n" +
                "Trân trọng,\n" +
                "Đội ngũ hỗ trợ";

                var heml = _emailService.GetEmailHTML("Đơn xin việc của bạn đã bị từ chối!", $"Chào {user.FullName},\n\n", body);

                await _emailService.SendEmailAsyncWithHTML(user.Email, "Đơn xin việc của bạn đã bị từ chối!", heml);
            }
            else if (newStatus == 3)
            {
                string body =

                "Chi tiết công việc đã ứng tuyển:\n" +
                $"Tiêu đề: {postJob.JobTitle}\n" +
                $"Mô tả: {postJob.JobDescription}\n" +
                "Hãy thường xuyên theo dõi để cập nhật thông tin mới nhất.\n\n" +
                "Chúc bạn sớm được nhận được việc làm .\n\n" +
                "Trân trọng,\n" +
                "Đội ngũ hỗ trợ";

                var heml = _emailService.GetEmailHTML("Nhà tuyển dụng đã xem hồ sơ của bạn!", $"Chào {user.FullName},\n\n", body);
                await _emailService.SendEmailAsyncWithHTML(user.Email, "Nhà tuyển dụng đã xem hồ sơ của bạn!", heml);
            }
            else if (newStatus == 4)
            {
                string body =

                "Chi tiết công việc đã ứng tuyển:\n" +
                $"Tiêu đề: {postJob.JobTitle}\n" +
                $"Mô tả: {postJob.JobDescription}\n" +
                "Chúc bạn làm việc vui vẻ.\n\n" +
                "Trân trọng,\n" +
                "Đội ngũ hỗ trợ";

                var heml = _emailService.GetEmailHTML("Nhà tuyển dụng đã nhận bạn !", $"Chào {user.FullName},\n\n", body);
                await _emailService.SendEmailAsyncWithHTML(user.Email, "Bạn đã được nhận vào làm!", heml);
            }
            else if (newStatus == 5)
            {
                string body =

                "Sau khi xem đơn xin việc thì nhà tuyển dụng đã từ chối bạn.\n" +
                "Chi tiết công việc đã ứng tuyển:\n" +
                $"Tiêu đề: {postJob.JobTitle}\n" +
                $"Mô tả: {postJob.JobDescription}\n" +
                "Đừng nản lòng , bạn vẫn có thể ứng tuyển vào các công việc khác.\n\n" +
                "Trân trọng,\n" +
                "Đội ngũ hỗ trợ";


                var heml = _emailService.GetEmailHTML("Nhà tuyển dụng đã từ chối bạn !", $"Chào {user.FullName},\n\n", body);
                await _emailService.SendEmailAsyncWithHTML(user.Email, "Nhà tuyển dụng đã từ chối bạn !", heml);
            }


            return await _applyJoBService.ChangeStatusOfJobseekerApply(Applyjob_Id, newStatus);

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
            /// check tên claim
            //foreach (var claim in jwtToken.Claims)
            //{
            //    Console.WriteLine($"Claim Type: {claim.Type}, Claim Value: {claim.Value}");
            //}
            /// check tên claim
            var userIdClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "nameid");

            if (userIdClaim == null)
            {
                throw new Exception("User ID not found in token.");
            }

            return userIdClaim.Value;
        }
    }
}
