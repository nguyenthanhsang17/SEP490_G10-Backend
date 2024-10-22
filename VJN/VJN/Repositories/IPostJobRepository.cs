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
        public Task<int> CountApplyJob(int jobId);
    }
}
