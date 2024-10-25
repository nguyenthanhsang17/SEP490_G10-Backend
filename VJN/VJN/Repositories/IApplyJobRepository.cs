using System.Reflection.Metadata;
using VJN.Models;

namespace VJN.Repositories
{
    public interface IApplyJobRepository
    {
        public Task<IEnumerable<ApplyJob>> getApplyJobByJobSeekerId(int JobSeekerId);

        public Task<IEnumerable<ApplyJob>> getApplyJobByPostId(int postId);

        public Task<bool> ChangeStatusOfJobseekerApply(int ApplyJobId, int newStatus);

        public Task<bool> CancelApplyJob(int postjob, int userid);

        public Task<bool> ApplyJob(ApplyJob applyJob);
    }

}
