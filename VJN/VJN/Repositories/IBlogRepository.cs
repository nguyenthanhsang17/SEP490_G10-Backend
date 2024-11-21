using System.Reflection.Metadata;
using VJN.Models;

namespace VJN.Repositories
{
    public interface IBlogRepository
    {
        public Task<IEnumerable<Blog>> getThreeBlogNews();
        public Task<bool> CreateBlog(string title, string description, int thumbnailId, int authorId);
        public  Task<bool> ChangeStatusBlog(int blogId, int newStatus);
    }
}
