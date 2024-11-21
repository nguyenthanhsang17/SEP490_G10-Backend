using AutoMapper;
using VJN.Models;
using VJN.ModelsDTO.EmployerDTOs;
using VJN.ModelsDTO.PostJobDTOs;
using VJN.Paging;
using VJN.Repositories;

namespace VJN.Services
{
    public class EmployerService : IEmployerService
    {
        public readonly IUserRepository _userRepository;
        public readonly IPostJobRepository _postJobRepository;
        public readonly IMapper _mapper;
        const double EarthRadiusKm = 6371.0;
        public EmployerService(IUserRepository userRepository, IMapper mapper, IPostJobRepository postJobRepository)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _postJobRepository = postJobRepository;
        }

        private static double DegreesToRadians(double degrees)
        {
            return degrees * (Math.PI / 180);
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

        private double CalculateDistance(decimal? lat1, decimal? lon1, decimal? lat2, decimal? lon2)
        {
            if (!lat1.HasValue || !lon1.HasValue || !lat2.HasValue || !lon2.HasValue)
            {
                return 0; // Or any default value you prefer
            }
            var result = Math.Abs(Haversine(lat1.Value, lon1.Value, lat2.Value, lon2.Value));
            return Math.Round(result, 2);
        }

        public async Task<EmployerDTO> GetEmployerByUserId(int id, int? userid, decimal? Latitude, decimal? Longitude, int pagenumber)
        {
            int pagesize = 6;
            var user = await _userRepository.findById(id);
            var employerdto = _mapper.Map<EmployerDTO>(user);


            var postjob = await _postJobRepository.GetPostJobBuAuthorid(id);

            PagedResult<PostJob> postjobPage = PaginationHelper.GetPaged<PostJob>(postjob, pagenumber, pagesize);


            var jobSearchResultTasks = postjobPage.Items.Select(async j => new JobSearchResult
            {
                PostId = j.PostId,
                thumbnail = j.ImagePostJobs.Count() == 0 || j.ImagePostJobs == null ? "" : j.ImagePostJobs.ElementAt(0).Image.Url,
                JobTitle = j.JobTitle,
                Salary = j.Salary,
                NumberPeople = j.NumberPeople,
                Address = j.Address,
                Latitude = j.Latitude,
                Longitude = j.Longitude,
                distance = CalculateDistance(Latitude, Longitude, j.Latitude, j.Longitude),
                AuthorName = j.Author.FullName,
                SalaryTypeName = j.SalaryTypes.TypeName,
                JobCategoryName = j.JobCategory.JobCategoryName,
                ExpirationDate = j.ExpirationDate,
                IsUrgentRecruitment = j.IsUrgentRecruitment,
                NumberOfApplicants = j.ApplyJobs.Count(),
                isWishlist = j.WishJobs.Where(wj => userid != 0 && wj.JobSeekerId == userid && wj.PostJobId == j.PostId).Count(),
            }).ToList();

            var jobSearchResult = await Task.WhenAll(jobSearchResultTasks);
            employerdto.PostJobAuthors = new PagedResult<JobSearchResult>(jobSearchResult.ToList(), postjob.Count(), pagenumber, pagesize);

            return employerdto;
        }
    }
}
