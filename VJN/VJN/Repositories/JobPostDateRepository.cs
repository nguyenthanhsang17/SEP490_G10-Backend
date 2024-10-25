using VJN.Models;
using VJN.ModelsDTO.JobPostDateDTOs;

namespace VJN.Repositories
{
    public class JobPostDateRepository : IJobPostDateRepository
    {
        private readonly VJNDBContext _context;
        public JobPostDateRepository(VJNDBContext context)
        {
            _context = context;
        }
        public async Task<bool> CreateJobPostDate(IEnumerable<JobPostDateCreateDTO> jobPostDateCreateDTOs)
        {
            foreach(var jobPostDateDTO in jobPostDateCreateDTOs)
            {
                var jobPostDate = new JobPostDate
                {
                    PostId = jobPostDateDTO.PostId,
                    EventDate = jobPostDateDTO.EventDate,
                    StartTime = jobPostDateDTO.StartTime,
                    EndTime = jobPostDateDTO.EndTime,
                };
                _context.JobPostDates.Add(jobPostDate);
                int i= await _context.SaveChangesAsync();
                if(i == 0)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
