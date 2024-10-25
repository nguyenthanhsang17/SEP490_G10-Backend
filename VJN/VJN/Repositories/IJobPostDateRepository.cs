using VJN.ModelsDTO.JobPostDateDTOs;

namespace VJN.Repositories
{
    public interface IJobPostDateRepository
    {
        public Task<bool> CreateJobPostDate(IEnumerable<JobPostDateCreateDTO> jobPostDateCreateDTOs);
    }
}
