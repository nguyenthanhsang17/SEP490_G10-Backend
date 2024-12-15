using AutoMapper;
using VJN.Models;
using VJN.ModelsDTO.ApplyJobDTOs;
using VJN.Repositories;

namespace VJN.Services
{
    public class ApplyJobService : IApplyJobService
    {
        private readonly IApplyJobRepository _applyJobRepository;
        private readonly IMapper _mapper;
        public ApplyJobService(IApplyJobRepository applyJobRepository, IMapper mapper)
        {
            _applyJobRepository = applyJobRepository;
            _mapper = mapper;
        }

        public async Task<bool> ApplyJob(ApplyJobCreateDTO applyJob, int uid)
        {
            var aj = _mapper.Map<ApplyJob>(applyJob);
            aj.JobSeekerId = uid;
            var c = await _applyJobRepository.ApplyJob(aj);
            return c;
        }

        public async Task<bool> CancelApplyJob(int postjob, int userid)
        {
            var c  = await _applyJobRepository.CancelApplyJob(postjob, userid);
            return c;
        }

        public async Task<bool> ChangeStatusOfJobseekerApply(int applyJobId, int newStatus)
        {
            return await _applyJobRepository.ChangeStatusOfJobseekerApply(applyJobId, newStatus);
        }

        public async Task<bool> checkReapply(int JobSeekerId, int postId)
        {
            var c = await _applyJobRepository.checkReapply(JobSeekerId, postId);
            return c;
        }

        public async Task<IEnumerable<ApplyJob>> getApplyJobByJobSeekerId(int JobSeekerId)
        {
            var applyjobs = await _applyJobRepository.getApplyJobByJobSeekerId(JobSeekerId);
            return applyjobs;
        }
        public async Task<IEnumerable<ApplyJob>> getApplyJobByPostId(int postId)
        {
            var applyjobs = await _applyJobRepository.getApplyJobByPostId(postId);
            return applyjobs;
        }

        public async Task<IEnumerable<ApplyJob>> GetApplyJobsByUserIdAndPostId(int JobSeekerId, int postId) 
        {
            var applyjobs = await _applyJobRepository.GetApplyJobsByUserIdAndPostId(JobSeekerId, postId);
            return applyjobs;
        }

        public async Task<bool> ReApplyJob(int applyjobid, int newCv) 
        {
            return await _applyJobRepository.ReApplyJob(applyjobid, newCv);
        }
    }
}
