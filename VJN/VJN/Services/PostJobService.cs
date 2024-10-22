using AutoMapper;
using VJN.Models;
using VJN.ModelsDTO.PostJobDTOs;
using VJN.Paging;
using VJN.Repositories;

namespace VJN.Services
{
    public class PostJobService : IPostJobService
    {
        //so luong job trong 1 trang
        const int PageSize = 3;
        const double EarthRadiusKm = 6371.0;

        private readonly IPostJobRepository _postJobRepository;
        private readonly IMapper _mapper;
        public PostJobService(IPostJobRepository postJobRepository, IMapper mapper) {
            _postJobRepository = postJobRepository;
            _mapper = mapper;
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

        public async Task<PagedResult<JobSearchResult>> SearchJobPopular(PostJobSearch postJobSearch, int pageNumber)
        {
            var jobsIds = await _postJobRepository.SearchJobPopular(postJobSearch);
            var pageIds = PaginationHelper.GetPaged<int>(jobsIds, pageNumber, PageSize);

            var jobs = await _postJobRepository.jobSearchResults(pageIds.Items);

            var jobSearchResultTasks =  jobs.Select(async j=> new JobSearchResult
            {
                PostId = j.PostId,
                thumbnail = await _postJobRepository.getThumnailJob(j.PostId),
                JobTitle = j.JobTitle,
                RangeSalaryMax = j.RangeSalaryMax,
                RangeSalaryMin = j.RangeSalaryMin,
                FixSalary = j.FixSalary,
                NumberPeople = j.NumberPeople,
                Address = j.Address,
                Latitude = j.Latitude,
                Longitude = j.Longitude,
                distance =  Math.Abs(Haversine(postJobSearch.Latitude.Value, postJobSearch.Longitude.Value, j.Latitude.Value, j.Longitude.Value)) , 
                AuthorName = j.Author.FullName,
                SalaryTypeName = j.SalaryTypes.TypeName,
                JobCategoryName = j.JobCategory.JobCategoryName,
                ExpirationDate = j.ExpirationDate,
                IsUrgentRecruitment = j.IsUrgentRecruitment,
                NumberOfApplicants = await _postJobRepository.CountApplyJob(j.PostId)

            }).ToList();

            var jobSearchResult = await Task.WhenAll(jobSearchResultTasks);
            var page = new PagedResult<JobSearchResult>(jobSearchResult, jobsIds.Count(), pageNumber, PageSize);
            return page;

        }
    }
}
