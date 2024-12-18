﻿using VJN.Models;
using VJN.ModelsDTO.ApplyJobDTOs;


namespace VJN.Services
{
    public interface IApplyJobService
    {
        
        public Task<IEnumerable<ApplyJob>> getApplyJobByJobSeekerId(int JobSeekerId);

        public Task<IEnumerable<ApplyJob>> getApplyJobByPostId(int postId);

        public Task<bool> ChangeStatusOfJobseekerApply(int applyJobId, int newStatus);
        public Task<bool> CancelApplyJob(int postjob, int userid);

        public Task<bool> ApplyJob(ApplyJobCreateDTO applyJob, int uid);
        public Task<IEnumerable<ApplyJob>> GetApplyJobsByUserIdAndPostId(int JobSeekerId, int postId);

        public Task<bool> checkReapply(int JobSeekerId, int postId);

        public  Task<bool> ReApplyJob(int applyjobid, int newCv);
    }
}
