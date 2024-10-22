using Microsoft.EntityFrameworkCore;
using VJN.Models;

namespace VJN.Repositories
{
    public class ApplyJobRepository : IApplyJobRepository
    {
        private readonly VJNDBContext _context;
        public ApplyJobRepository(VJNDBContext context)
        {
            _context = context;
        }

        public async Task<bool> ChangeStatusOfJobseekerApply(int ApplyJobId, int newStatus)
        {
                var applyjob =  await _context.ApplyJobs.FindAsync(ApplyJobId);
            if (applyjob != null)
            {
                applyjob.Status = newStatus;    
                _context.Entry(applyjob).State = EntityState.Modified;
                _context.SaveChanges();
                return true;
            }
            return false;

        }

        public async Task<IEnumerable<ApplyJob>> getApplyJobByJobSeekerId(int JobSeekerId)
        {
            var ApplyJobs = await _context.ApplyJobs.Where(aj=>aj.JobSeekerId == JobSeekerId).ToListAsync();
            return ApplyJobs;
        }
        
        public async Task<IEnumerable<ApplyJob>> getApplyJobByPostId(int postId)
        {
            var ApplyJobs = await _context.ApplyJobs.Where(aj => aj.PostId == postId).ToListAsync();
            return ApplyJobs;
        }

    }
}
