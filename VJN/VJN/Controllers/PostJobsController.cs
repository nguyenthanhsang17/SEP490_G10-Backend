using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Imagekit.Sdk;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;
using VJN.Models;
using VJN.ModelsDTO.MediaItemDTOs;
using VJN.ModelsDTO.PostJobDTOs;
using VJN.ModelsDTO.ReportDTO;
using VJN.Paging;
using VJN.Repositories;
using VJN.Services;

namespace VJN.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostJobsController : ControllerBase
    {
        private readonly IPostJobService _postJobService;
        private readonly ISlotService _slotService;

        private readonly VJNDBContext _context;


        private readonly ImagekitClient _imagekitClient;
        private readonly IMediaItemService _mediaItemService;
        private readonly IImagePostJobService _imagepostJobService;
        private readonly IJobPostDateService _jobPostDateService;
        private readonly IReportMediaServices _reportMediaService;
        public PostJobsController(IPostJobService postJobService, ISlotService slotService, IMediaItemService mediaItemService, IImagePostJobService imagepostJobService, IJobPostDateService jobPostDateService, IReportMediaServices reportMediaService, VJNDBContext context)
        {
            _postJobService = postJobService;
            _slotService = slotService;
            _imagekitClient = new ImagekitClient("public_Q+yi7A0O9A+joyXIoqM4TpVqOrQ=", "private_e2V3fNLKwK0pGwSrEmFH+iKQtks=", "https://ik.imagekit.io/ryf3sqxfn");
            _mediaItemService = mediaItemService;
            _imagepostJobService = imagepostJobService;
            _jobPostDateService = jobPostDateService;
            _reportMediaService = reportMediaService;
            _context = context;
        }


        [HttpGet]
        public async Task<ActionResult<PagedResult<JobSearchResult>>> GetPostJobsPopular([FromQuery] PostJobSearch model)
        {
            string id_str = GetUserIdFromToken();
            int userid = 0;
            if (!string.IsNullOrEmpty(id_str))
            {
                userid = int.Parse(id_str);
            }
            var jobs = await _postJobService.SearchJobPopular(model, userid);
            if (jobs == null || jobs.Items.Count() == 0)
            {
                return BadRequest(new { Message = "không tìm thấy công việc !!!" });
            }
            return Ok(jobs);
        }

        [HttpGet("jobDetails/{id}")]
        public async Task<ActionResult<PostJob>> getJobDetails(int id)
        {
            string? iduser_str = GetUserIdFromToken();
            int? iduser = null;
            if (!string.IsNullOrEmpty(iduser_str))
            {
                iduser = int.Parse(iduser_str);
            }

            var postdto = await _postJobService.getJostJobByID(id, iduser);

            var slotDTO = await _slotService.GetSlotByPostjobId(id);

            postdto.slotDTOs = slotDTO;
            return Ok(postdto);
        }
        [HttpPut("ShowPostJob/{id}")]
        public async Task<IActionResult> ShowPostJob(int id)
        {
            var c = await _postJobService.ChangeStatusPostJob(id, 2);
            if (c)
            {
                return Ok(c);
            }
            else
            {
                return BadRequest(new { Message = "Hiện bài post thất bại" });
            }
        }

        [HttpPut("HidePostJob/{id}")]
        public async Task<IActionResult> HidePostJob(int id)
        {
            var c = await _postJobService.ChangeStatusPostJob(id, 5);
            if (c)
            {
                return Ok(c);
            }
            else
            {
                return BadRequest(new { Message = "Ẩn bài post thất bại" });
            }
        }

        [Authorize]
        [HttpPost("CreatePost")]
        public async Task<IActionResult> CreatePostJob([FromBody] PostJobCreateDTO postJobCreateDTO)
        {
            Console.WriteLine("chay ham nay");
            string userid_str = GetUserIdFromToken();
            int uid = int.Parse(userid_str);
            var id = await _postJobService.CreatePostJob(postJobCreateDTO, uid);
            if (id <= 0)
            {
                return BadRequest(new { Message = "Lỗi Tạo Công Việc" });
            }
            return Ok(id);
        }

        [Authorize]
        [HttpGet("GetListJobsCreated")]
        public async Task<ActionResult<PagedResult<JobSearchResultEmployer>>> GetListJobsCreated([FromQuery] PostJobSearchEmployer s)
        {
            string userid = GetUserIdFromToken();
            int id = int.Parse(userid);
            var page = await _postJobService.GetJobListByEmployerID(id, s);
            return Ok(page);
        }

        private string GetUserIdFromToken()
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (string.IsNullOrEmpty(token))
            {
                return null;
            }

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            var userIdClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "nameid");

            if (userIdClaim == null)
            {
                return null;
            }

            return userIdClaim.Value;
        }

        [HttpPut("Accept/{id}")]
        public async Task<IActionResult> AcceptPostJob(int id)
        {
            var c = await _postJobService.ChangeStatusPostJob(id, 2);
            if (c)
            {
                return Ok(c);
            }
            else
            {
                return BadRequest(new { Message = "Duyệt bài đăng thất bại" });
            }
        }

        [HttpPut("Reject/{id}")]
        public async Task<IActionResult> RejectPostJob(int id, string reasonRejecr)
        {
            var c = await _postJobService.ChangeStatusPostJob(id, 3);
            //do something with reasonRejecr
            if (c)
            {
                return Ok(c);
            }
            else
            {
                return BadRequest(new { Message = "Từ chối bài đăng thất bại" });
            }
        }

        [HttpPut("Ban/{id}")]
        public async Task<IActionResult> BanPostJob(int id, string reasonBan)
        {
            var c = await _postJobService.ChangeStatusPostJob(id, 6);
            
            if (c)
            {
                var banlog = new BanLogPostJob
                {
                    Reason = "aa",
                    PostId = id,
                    AdminId = 1
                };
                _context.BanLogPostJobs.Add(banlog);
                 await _context.SaveChangesAsync();
                return Ok(c);
            }
            else
            {
                return BadRequest(new { Message = "Cấm bài viết thất bại " });
            }
        }
        [Authorize]
        [HttpPost("ReportJob")] 
        public async Task<ActionResult<int>> ReportJob([FromForm] ReportCreateDTO model)
        {
            var id_str = GetUserIdFromToken();
            var userid = int.Parse(id_str);

            var reportid = await _postJobService.ReportJob(model, userid);

            if(reportid<=0)
            {
                return BadRequest(new { Message = "Bạn đã Report 3 lần trong ngày" });
            }           

            if (model.files == null || !model.files.Any())
                return BadRequest("No files provided.");

            var mediaIds = new List<int>();

            var uploadTasks = model.files.Select(async file =>
            {
                if (file.Length == 0)
                    throw new ArgumentException("One or more files are empty.");

                using (var memoryStream = new System.IO.MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);
                    byte[] fileBytes = memoryStream.ToArray();

                    FileCreateRequest uploadRequest = new FileCreateRequest
                    {
                        file = fileBytes,
                        fileName = file.FileName
                    };

                    Result result = _imagekitClient.Upload(uploadRequest);
                    var media = new MediaItemDTO
                    {
                        Url = result.url,
                        Status = true
                    };
                    return await _mediaItemService.CreateMediaItem(media);
                }
            });

            try
            {
                mediaIds = (await Task.WhenAll(uploadTasks)).ToList();
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, $"Upload failed: {ex.Message}");
            }
            var result = await _reportMediaService.CreateReportMedia(reportid, mediaIds);
            return Ok(result);
        }

        [HttpPut("UnBan/{id}")]
        public async Task<IActionResult> RUnBanPostJob(int id)
        {
            var c = await _postJobService.ChangeStatusPostJob(id, 2);
            if (c)
            {
                return Ok(c);
            }
            else
            {
                return BadRequest(new { Message = "Baì viết đã được gỡ lệnh cấm" });
            }
        }

        [HttpGet("GetAllPostJobs")]
        public async Task<ActionResult<PagedResult<PostJobDTOforReport>>> GetAllPostJobs([FromQuery] int status, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {

            IEnumerable<PostJobDTOforReport> postJobs;

            // Lấy dữ liệu từ dịch vụ dựa trên giá trị của status
            if (status == 1)
            {
                // Lấy tất cả các bài đăng với status 1 (yêu cầu duyệt)
                postJobs = await _postJobService.GetAllPostJobByStatus(1);
            }
            else if (status == -1)
            {
                // Lấy tất cả các bài đăng
                postJobs = await _postJobService.GetAllPostJobByStatus(-1);
            }
            //else if (status == 2)
            //{
            //    // Lấy tất cả các bài đăng theo trang thai khac
            //    postJobs = await _postJobService.GetAllPostJobByStatus(stt);
            //}
            else 
            {
                // Lấy các bài đăng có báo cáo (Reports)
                postJobs = (await _postJobService.GetAllPostJobByStatus(-1))
                            .Where(item => item.Reports != null && item.Reports.Any());
            }
            var pagedResult = postJobs.GetPaged(pageNumber, pageSize);

            return Ok(pagedResult);
        }
        

        [HttpGet("GetPostDetailForStaff")]
        public async Task<IActionResult> GetPostDetailForStaff(int id)
        {
            var post = await _postJobService.GetPostByIDForStaff(id);
            if (post!=null)
            {
                return Ok(post);
            }
            else
            {
                return BadRequest(new { Message = "Bài đăng ko tồn tại" });
            }
        }

    }
}
