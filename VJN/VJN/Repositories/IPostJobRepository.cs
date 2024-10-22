using VJN.Models;
using VJN.ModelsDTO.PostJobDTOs;

namespace VJN.Repositories
{
    public interface IPostJobRepository
    {
        public Task<IEnumerable<PostJob>> GetPorpularJob();
<<<<<<< HEAD
        Task<string?> getThumnailJob(int postId);
        public Task<IEnumerable<int>> SearchJobPopular(PostJobSearch postJobSearch);
        public Task<IEnumerable<PostJob>> jobSearchResults(IEnumerable<int> jobIds);
        public Task<int> CountApplyJob(int jobId);
=======
        public Task<string> getThumnailJob(int id);

        public Task<PostJob> GetPostJobById(int id);
>>>>>>> 6ffb2ae (Inter 1)
    }
}
