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

        public async Task<bool> DeleteAllJobPostDate(int postid)
        {
            var c=await _jobPostDateRepository.DeleteAllJobPostByPOstID(postid);
            return c;
        }

        public async Task<bool> UpdateJobPostDate(int postid, IEnumerable<JobPostDateForUpdateDTO> jobPostDates)
        {
            var c1 = await _jobPostDateRepository.DeleteAllJobPostByPOstID(postid);
            Console.WriteLine("c1: "+c1);
            if (c1)
            {
                var c2 = await _jobPostDateRepository.UpdateAllDate(postid, jobPostDates);
                Console.WriteLine("c2: "+c2);
                return c2;
            }
            return false;
        }
    }
}
