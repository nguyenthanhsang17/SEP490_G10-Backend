using VJN.ModelsDTO.JobPostDateDTOs;
using VJN.Repositories;

namespace VJN.Services
{
    public class JobPostDateService : IJobPostDateService
    {
        public readonly IJobPostDateRepository _jobPostDateRepository;

        public JobPostDateService(IJobPostDateRepository jobPostDateRepository)
        {
            _jobPostDateRepository = jobPostDateRepository;
        }

        public async Task<bool> CreateJobPostDate(IEnumerable<JobPostDateCreateDTO> jobPostDateCreateDTOs)
        {
            var c = await _jobPostDateRepository.CreateJobPostDate(jobPostDateCreateDTOs);
            return c;
        }
    }
}
