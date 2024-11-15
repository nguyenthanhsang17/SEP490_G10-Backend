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

        public async Task<bool> ApplyJob(ApplyJob applyJob)
        {
            var c  = await _context.ApplyJobs.Where(aj=>aj.JobSeekerId==applyJob.JobSeekerId&&aj.PostId==applyJob.PostId&&aj.Status!=6).AnyAsync();
            if(c)
            {
                return false;
            }
            else
            {
                _context.ApplyJobs.Add(applyJob);
                await _context.SaveChangesAsync();
                return true;
            }
        }

        public async Task<bool> CancelApplyJob(int postjob, int userid)
        {
            var aj = await _context.ApplyJobs.Where(aj=>aj.JobSeekerId==userid&&aj.PostId==postjob).SingleOrDefaultAsync();
            if(aj!=null)
            {
                if(aj.Status==3|| aj.Status == 2|| aj.Status == 0)
                {

                    aj.Status = 6;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
            _context.Entry(aj).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return true;
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
