﻿using VJN.Models;
using VJN.ModelsDTO.PostJobDTOs;

namespace VJN.Repositories
{
    public interface IPostJobRepository
    {
        public Task<IEnumerable<PostJob>> GetPorpularJob();
        Task<string?> getThumnailJob(int postId);
        public Task<IEnumerable<int>> SearchJobPopular(PostJobSearch postJobSearch);
        public Task<IEnumerable<PostJob>> jobSearchResults(IEnumerable<int> jobIds);
        public Task<PostJob> getJostJobByID(int id);
        public Task<int> CountApplyJob(int jobId);
        public Task<PostJob> GetPostJobById(int id);

        public Task<IEnumerable<string>> getAllImageJobByJobId(int jid);
        public Task<bool> GetisAppliedJob(int jid, int userid);
        public Task<bool> GetisWishJob(int jid, int userid);
        public Task<bool> ChangeStatusPostJob(int jobID, int status);

        public Task<int> CreatePostJob(PostJob postJob);

        public Task<IEnumerable<int>> GetPostJobCreatedByEmployerID(int employerID, PostJobSearchEmployer search);

        public Task<IEnumerable<PostJob>> GetAllPostJob(int status);

        public Task<bool> AddWishJob(int jobid, int userid);
        public Task<bool> DeleteWishJob(int jobid, int userid);

        public Task<IEnumerable<int>> getJobIdInWishList(PostJobSearchWishList s, int userid);
        public Task<int> ReportJob(Report report);
    }
}
