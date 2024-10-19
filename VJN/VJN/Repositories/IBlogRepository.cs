using System.Reflection.Metadata;
using VJN.Models;

namespace VJN.Repositories
{
    public interface IBlogRepository
    {
        public Task<IEnumerable<Blog>> getThreeBlogNews();
    }
}
