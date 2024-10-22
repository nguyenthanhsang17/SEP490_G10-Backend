using VJN.Models;


namespace VJN.Services
{
    public interface IApplyJobService
    {
        
        public Task<IEnumerable<ApplyJob>> getApplyJobByJobSeekerId(int JobSeekerId);

        public Task<IEnumerable<ApplyJob>> getApplyJobByPostId(int postId);

        public Task<bool> ChangeStatusOfJobseekerApply(int applyJobId, int newStatus);
    }
}
