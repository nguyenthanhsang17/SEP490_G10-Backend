using VJN.ModelsDTO.JobPostDateDTOs;

namespace VJN.Services
{
    public interface IJobPostDateService
    {
        public Task<bool> CreateJobPostDate(IEnumerable<JobPostDateCreateDTO> jobPostDateCreateDTOs);

        public Task<bool> UpdateJobPostDate(int postid, IEnumerable<JobPostDateForUpdateDTO> jobPostDates);
        public Task<bool> DeleteAllJobPostDate(int postid);
    }
}
