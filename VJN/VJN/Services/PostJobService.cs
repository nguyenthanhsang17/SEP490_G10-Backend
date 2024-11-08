using AutoMapper;
<<<<<<< HEAD
using Microsoft.EntityFrameworkCore;
=======
using AutoMapper.Execution;
>>>>>>> 79fa441ff4e50fcad1fc1cf0fb84c14fb9c45118
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SQLitePCL;
using VJN.Models;
using VJN.ModelsDTO.PostJobDTOs;
using VJN.ModelsDTO.ReportDTO;
using VJN.ModelsDTO.UserDTOs;
using VJN.Paging;
using VJN.Repositories;

namespace VJN.Services
{
    public class PostJobService : IPostJobService
    {
        //so luong job trong 1 trang
        const int PageSize = 6;
        const double EarthRadiusKm = 6371.0;

        private readonly IPostJobRepository _postJobRepository;
        private readonly IServicePriceLogRepository _priceLogRepository;
        private readonly IMapper _mapper;
<<<<<<< HEAD
        private readonly IUserService _userService;
        private readonly VJNDBContext _context;
        public PostJobService(IPostJobRepository postJobRepository, IMapper mapper, IServicePriceLogRepository priceLogRepository, VJNDBContext context, IUserService userService)
=======
        private readonly IImagePostJobRepository _imagePostJobRepository;
        private readonly ISlotRepository _slotRepository;
        private readonly IJobPostDateRepository _jobPostDateRepository;
        public PostJobService(IPostJobRepository postJobRepository, IMapper mapper, IServicePriceLogRepository priceLogRepository, IImagePostJobRepository imagePostJobRepository, ISlotRepository slotRepository, IJobPostDateRepository jobPostDateRepository)
>>>>>>> 79fa441ff4e50fcad1fc1cf0fb84c14fb9c45118
        {
            _postJobRepository = postJobRepository;
            _mapper = mapper;
            _priceLogRepository = priceLogRepository;
<<<<<<< HEAD
            _context = context;
            _userService = userService;
=======
            _imagePostJobRepository = imagePostJobRepository;
            _slotRepository = slotRepository;
            _jobPostDateRepository = jobPostDateRepository;
>>>>>>> 79fa441ff4e50fcad1fc1cf0fb84c14fb9c45118
        }
        public async Task<IEnumerable<PostJobDTOForHomepage>> getPorpularJob()
        {
            var p = await _postJobRepository.GetPorpularJob();
            var pdto = _mapper.Map<IEnumerable<PostJobDTOForHomepage>>(p);
            foreach (var dto in pdto)
            {
                dto.thumnail = await _postJobRepository.getThumnailJob(dto.PostId);
            }
            return pdto;
        }


        public double Haversine(decimal lat1, decimal lon1, decimal lat2, decimal lon2)
        {
            double dLat = DegreesToRadians((double)(lat2 - lat1));
            double dLon = DegreesToRadians((double)(lon2 - lon1));

            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                       Math.Cos(DegreesToRadians((double)lat1)) * Math.Cos(DegreesToRadians((double)lat2)) *
                       Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            return EarthRadiusKm * c;
        }

        private static double DegreesToRadians(double degrees)
        {
            return degrees * (Math.PI / 180);
        }

        public async Task<PagedResult<JobSearchResult>> SearchJobPopular(PostJobSearch postJobSearch, int? userid)
        {
            var jobsIds = await _postJobRepository.SearchJobPopular(postJobSearch);

            var pageIds = PaginationHelper.GetPaged<int>(jobsIds, postJobSearch.pageNumber, PageSize);

            var jobs = await _postJobRepository.jobSearchResults(pageIds.Items);

            var jobSearchResultTasks = jobs.Select(async j => new JobSearchResult
            {
                PostId = j.PostId,
                thumbnail = j.ImagePostJobs.Count() == 0 || j.ImagePostJobs == null ? "" : j.ImagePostJobs.ElementAt(0).Image.Url,
                JobTitle = j.JobTitle,
                Salary = j.Salary,
                NumberPeople = j.NumberPeople,
                Address = j.Address,
                Latitude = j.Latitude,
                Longitude = j.Longitude,
                distance = CalculateDistance(postJobSearch.Latitude, postJobSearch.Longitude, j.Latitude, j.Longitude),
                AuthorName = j.Author.FullName,
                SalaryTypeName = j.SalaryTypes.TypeName,
                JobCategoryName = j.JobCategory.JobCategoryName,
                ExpirationDate = j.ExpirationDate,
                IsUrgentRecruitment = j.IsUrgentRecruitment,
                NumberOfApplicants = j.ApplyJobs.Count(),
                isWishlist =  j.WishJobs.Where(wj=> userid != 0&&wj.JobSeekerId==userid&&wj.PostJobId==j.PostId).Count(),
            }).ToList();

            var jobSearchResult = await Task.WhenAll(jobSearchResultTasks);
            var page = new PagedResult<JobSearchResult>(jobSearchResult, jobsIds.Count(), postJobSearch.pageNumber, PageSize);
            return page;
        }
        private double CalculateDistance(decimal? lat1, decimal? lon1, decimal? lat2, decimal? lon2)
        {
            if (!lat1.HasValue || !lon1.HasValue || !lat2.HasValue || !lon2.HasValue)
            {
                return 0; // Or any default value you prefer
            }
            var result = Math.Abs(Haversine(lat1.Value, lon1.Value, lat2.Value, lon2.Value));
            return Math.Round(result, 2);
        }




        public async Task<PostJobDTOForList> GetPostJobById(int id)
        {
            PostJob postJob = await _postJobRepository.GetPostJobById(id);
            PostJobDTOForList postJobDTO = _mapper.Map<PostJobDTOForList>(postJob);
            return postJobDTO;

        }

        public async Task<PostJobDetailDTO> getJostJobByID(int id, int? userid)
        {
            var post = await _postJobRepository.getJostJobByID(id);
            var postdto = _mapper.Map<PostJobDetailDTO>(post);

            postdto.ImagePostJobs = await _postJobRepository.getAllImageJobByJobId(postdto.PostId);
            if (userid.HasValue)
            {
                postdto.isAppliedJob = await _postJobRepository.GetisAppliedJob(postdto.PostId, userid.Value);
                postdto.isWishJob = await _postJobRepository.GetisWishJob(postdto.PostId, userid.Value);
            }
            else
            {
                postdto.isAppliedJob = false;
                postdto.isWishJob = false;
            }
            postdto.NumberAppliedUser = await _postJobRepository.CountApplyJob(postdto.PostId);

            return postdto;
        }

        public async Task<bool> ChangeStatusPostJob(int jobID, int status)
        {
            var check = await _postJobRepository.ChangeStatusPostJob(jobID, status);
            return check;
        }

        public async Task<int> CreatePostJob(PostJobCreateDTO postJobdto, int u)
        {
            var postjob = _mapper.Map<PostJob>(postJobdto);
            postjob.AuthorId = u;
            int id = await _postJobRepository.CreatePostJob(postjob);
            return id;
        }

        public async Task<PagedResult<JobSearchResultEmployer>> GetJobListByEmployerID(int employerID, PostJobSearchEmployer s)
        {
            var id = await _postJobRepository.GetPostJobCreatedByEmployerID(employerID, s);

            var pageIds = PaginationHelper.GetPaged<int>(id, s.pageNumber, PageSize);

            var jobs = await _postJobRepository.jobSearchResults(pageIds.Items);
            var jobSearchResultTasks = jobs.Select(async j => new JobSearchResultEmployer
            {
                PostId = j.PostId,
                thumbnail = j.ImagePostJobs.Count() == 0 || j.ImagePostJobs == null ? "" : j.ImagePostJobs.ElementAt(0).Image.Url,
                JobTitle = j.JobTitle,
                Salary = j.Salary,
                NumberPeople = j.NumberPeople,
                Address = j.Address,
                Status = j.Status,
                Latitude = j.Latitude,
                Longitude = j.Longitude,
                CreateDate = j.CreateDate,
                SalaryTypeName = j.SalaryTypes.TypeName,
                JobCategoryName = j.JobCategory.JobCategoryName,
                ExpirationDate = j.ExpirationDate,
                IsUrgentRecruitment = j.IsUrgentRecruitment,
                NumberOfApplicants = j.ApplyJobs.Count(),
            }).ToList();
            var jobSearchResult = await Task.WhenAll(jobSearchResultTasks);
            var page = new PagedResult<JobSearchResultEmployer>(jobSearchResult, id.Count(), s.pageNumber, PageSize);
            return page;
        }

        public async Task<IEnumerable<PostJobDTOforReport>> GetAllPostJobByStatus(int status)
        {
            var postJobs = await _postJobRepository.GetAllPostJob(status);
            var result = _mapper.Map<IEnumerable<PostJobDTOforReport>>(postJobs);

            foreach (var postJobDto in result)
            {
                var postJobDTOForReport = await _context.PostJobs.FindAsync(postJobDto.PostId);
                string authorName = "";
                int? authorid = postJobDTOForReport.AuthorId;
                if (authorid.HasValue)
                {
                    UserDTO user = await _userService.findById(authorid.Value);
                    authorName = user.FullName;
                }
                postJobDto.AuthorName = authorName;


                var salarytype = await _context.SalaryTypes.FindAsync(postJobDTOForReport.SalaryTypesId);
                string salaryName = salarytype.TypeName;
                postJobDto.SalaryTypeName = salaryName;

                var jobcategory = await _context.JobCategories.FindAsync(postJobDTOForReport.JobCategoryId);
                string jobcategoryname = jobcategory.JobCategoryName;
                postJobDto.JobCategoryName = jobcategoryname;

                var imgPost = await _context.ImagePostJobs.Where(p => p.PostId == postJobDto.PostId).ToListAsync();
                var imgMedia = imgPost.Select(img => img.ImageId).ToList();
                List<String> images = new List<String>();
                foreach (var img in imgPost)
                {
                    var mediaitem = await _context.MediaItems.FindAsync(img.ImageId);
                    images.Add(mediaitem.Url);
                }
                postJobDto.ImagePostJobs = images;


                if (postJobDto.Reports != null && postJobDto.Reports.Any())
                {
                    postJobDto.Reports = postJobDto.Reports.Select(report => _mapper.Map<ReportDTO>(report)).ToList();
                }
            }

            return result;
        }

<<<<<<< HEAD
        public async Task<PostJobDTOReport?> GetPostByIDForStaff(int id)
        {
            var postJobDTO = await _context.PostJobs.Include(p=>p.ImagePostJobs).ThenInclude(i=>i.Image)
                .Include(p=>p.Author).ThenInclude(a=>a.AvatarNavigation)
                .Where(p => p.PostId == id)
                .Select(p => new PostJobDTOReport
                {
                    PostId = p.PostId,
                    JobTitle = p.JobTitle,
                    JobDescription = p.JobDescription,
                    Salary = p.Salary,
                    NumberPeople = p.NumberPeople,
                    Address = p.Address,
                    CreateDate = p.CreateDate,
                    ExpirationDate = p.ExpirationDate,
                    Status = p.Status,
                    CensorDate = p.CensorDate,

                    // Các thuộc tính từ bảng liên quan, chỉ lấy những gì cần thiết
                    Author = p.Author != null ? new UserDTOReport { UserId = p.Author.UserId, FullName = p.Author.FullName, Phonenumber = p.Author.Phonenumber, Age = p.Author.Age, Email = p.Author.Email, AvatarURL = p.Author.AvatarNavigation.Url } : null,
                    Censor = p.Censor != null ? new UserDTOReport { UserId = p.Censor.UserId, FullName = p.Censor.FullName, Phonenumber = p.Censor.Phonenumber, Age = p.Censor.Age, Email = p.Censor.Email, AvatarURL = p.Censor.AvatarNavigation.Url } : null,
                    JobCategory = p.JobCategory != null ? new JobCategory { JobCategoryId = p.JobCategory.JobCategoryId, JobCategoryName = p.JobCategory.JobCategoryName } : null,
                    SalaryTypes = p.SalaryTypes != null ? new SalaryType { SalaryTypesId = p.SalaryTypes.SalaryTypesId, TypeName = p.SalaryTypes.TypeName } : null,

                    // Chọn các thuộc tính cần thiết từ các bảng con có nhiều bản ghi
                    ImagePostJobs = p.ImagePostJobs,

                })
                .FirstOrDefaultAsync();

            return postJobDTO;
        }

=======
        public async Task<bool> AddWishJob(int jobid, int userid)
        {
            var c  = await _postJobRepository.AddWishJob(jobid, userid);
            return c;
        }

        public async Task<bool> DeleteWishJob(int jobid, int userid)
        {
            var c  = await _postJobRepository.DeleteWishJob(jobid, userid);
            return c;
        }

        public async Task<PagedResult<JobSearchResult>> getJobWishList(PostJobSearchWishList s, int userid)
        {
            var ids = await _postJobRepository.getJobIdInWishList(s, userid);

            var pageIds = PaginationHelper.GetPaged<int>(ids, s.pageNumber, PageSize);
            var jobs = await _postJobRepository.jobSearchResults(pageIds.Items);
            var jobSearchResultTasks = jobs.Select(async j => new JobSearchResult
            {
                PostId = j.PostId,
                thumbnail = j.ImagePostJobs.Count() == 0 || j.ImagePostJobs == null ? "" : j.ImagePostJobs.ElementAt(0).Image.Url,
                JobTitle = j.JobTitle,
                Salary = j.Salary,
                NumberPeople = j.NumberPeople,
                Address = j.Address,
                Latitude = j.Latitude,
                Longitude = j.Longitude,
                distance = CalculateDistance(s.Latitude, s.Longitude, j.Latitude, j.Longitude),
                AuthorName = j.Author.FullName,
                SalaryTypeName = j.SalaryTypes.TypeName,
                JobCategoryName = j.JobCategory.JobCategoryName,
                ExpirationDate = j.ExpirationDate,
                IsUrgentRecruitment = j.IsUrgentRecruitment,
                NumberOfApplicants = j.ApplyJobs.Count(),

            }).ToList();

            var jobSearchResult = await Task.WhenAll(jobSearchResultTasks);
            var page = new PagedResult<JobSearchResult>(jobSearchResult, ids.Count(), s.pageNumber, PageSize);
            return page;

        }

        public async Task<int> ReportJob(ReportCreateDTO report, int userid)
        {
            var reportModel = new Report()
            {
                JobSeekerId = userid,
                Reason = report.Reason,
                PostId = report.PostId,
                CreateDate = DateTime.Now,
                Status = 1
            };

            var i = await _postJobRepository.ReportJob(reportModel);
            return i;
        }
>>>>>>> 79fa441ff4e50fcad1fc1cf0fb84c14fb9c45118
    }
}
