using System.Reflection.Metadata;
using VJN.Models;

namespace VJN.Repositories
{
    public interface IBlogRepository
    {
        public Task<IEnumerable<Blog>> getThreeBlogNews();
        public Task<IEnumerable<Blog>> GetAllBlog();
        public Task<Blog> GetBlogDetail(int id);
    }
}
