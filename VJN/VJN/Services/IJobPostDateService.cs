using VJN.ModelsDTO.JobPostDateDTOs;

namespace VJN.Services
{
    public interface IJobPostDateService
    {
        public Task<bool> CreateJobPostDate(IEnumerable<JobPostDateCreateDTO> jobPostDateCreateDTOs);
    }
}
