using AutoMapper;
using AutoMapper.Execution;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using VJN.Models;
using VJN.ModelsDTO.PostJobDTOs;
using VJN.ModelsDTO.ReportDTO;
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
        private readonly IImagePostJobRepository _imagePostJobRepository;
        private readonly ISlotRepository _slotRepository;
        private readonly IJobPostDateRepository _jobPostDateRepository;
        public PostJobService(IPostJobRepository postJobRepository, IMapper mapper, IServicePriceLogRepository priceLogRepository, IImagePostJobRepository imagePostJobRepository, ISlotRepository slotRepository, IJobPostDateRepository jobPostDateRepository)
        {
            _postJobRepository = postJobRepository;
            _mapper = mapper;
            _priceLogRepository = priceLogRepository;
            _imagePostJobRepository = imagePostJobRepository;
            _slotRepository = slotRepository;
            _jobPostDateRepository = jobPostDateRepository;
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

        public async Task<PagedResult<JobSearchResult>> SearchJobPopular(PostJobSearch postJobSearch)
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

        public async Task<IEnumerable<PostJobDTOforReport>> GetAllPostJob(int status)
        {
            var postJobs = await _postJobRepository.GetAllPostJob(status);
            var result = _mapper.Map<IEnumerable<PostJobDTOforReport>>(postJobs);

            foreach (var postJobDto in result)
            {

                if (postJobDto.Reports != null && postJobDto.Reports.Any())
                {
                    postJobDto.Reports = postJobDto.Reports.Select(report => _mapper.Map<ReportDTO>(report)).ToList();
                }
            }

            return result;
        }

    }
}
