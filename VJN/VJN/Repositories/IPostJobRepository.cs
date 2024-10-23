using VJN.Models;
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
    }
}
