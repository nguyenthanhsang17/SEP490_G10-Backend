using Microsoft.EntityFrameworkCore;
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

        public async Task<bool> DeleteAllJobPostByPOstID(int postid)
        {
            var jobpostdate =  await _context.JobPostDates.Where(jpd => jpd.PostId==postid).ToListAsync();
            if (jobpostdate.Any())
            {
                foreach(var jdp in jobpostdate)
                {
                    _context.JobPostDates.Remove(jdp);
                    int i = await _context.SaveChangesAsync();
                    if (i>0)
                    {
                        return false;
                    }
                }
                return true;
            }
            else
            {
                return true;
            }
        }

        public async Task<bool> UpdateAllDate(int postid, IEnumerable<JobPostDateForUpdateDTO> jobPostDates)
        {
            foreach (var jobPostDateDTO in jobPostDates)
            {
                var jobPostDate = new JobPostDate
                {
                    PostId = postid,
                    EventDate = jobPostDateDTO.EventDate,
                    StartTime = jobPostDateDTO.StartTime,
                    EndTime = jobPostDateDTO.EndTime,
                };
                _context.JobPostDates.Add(jobPostDate);
                int i = await _context.SaveChangesAsync();
                if (i == 0)
                {
                    return false;
                }
            }
            return true;
        }

        public async Task<IEnumerable<JobPostDate>> GetPostJobByPostID(int postid)
        {
            var postjobdate = await _context.JobPostDates.Where(pj => pj.PostId == postid).ToListAsync();
            return postjobdate;
        }
    }
}
