using VJN.Models;

namespace VJN.Repositories
{
    public interface IPostJobRepository
    {
        public Task<IEnumerable<PostJob>> GetPorpularJob();
        public Task<string> getThumnailJob(int id);
    }
}
