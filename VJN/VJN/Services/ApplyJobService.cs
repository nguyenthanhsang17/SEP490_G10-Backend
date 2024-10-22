using AutoMapper;
using VJN.Models;
using VJN.Repositories;

namespace VJN.Services
{
    public class ApplyJobService : IApplyJobService
    {
        private readonly IApplyJobRepository _applyJobRepository;
        public ApplyJobService(IApplyJobRepository applyJobRepository)
        {
            _applyJobRepository = applyJobRepository;
        }

        public async Task<bool> ChangeStatusOfJobseekerApply(int applyJobId, int newStatus)
        {
            return await _applyJobRepository.ChangeStatusOfJobseekerApply(applyJobId, newStatus);
        }

        public async Task<IEnumerable<ApplyJob>> getApplyJobByJobSeekerId(int JobSeekerId)
        {
            var applyjobs = await _applyJobRepository.getApplyJobByJobSeekerId(JobSeekerId);
            return applyjobs;
        }
        public async Task<IEnumerable<ApplyJob>> getApplyJobByPostId(int postId)
        {
            var applyjobs = await _applyJobRepository.getApplyJobByJobSeekerId(postId);
            return applyjobs;
        }
    }
}
