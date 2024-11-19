using VJN.Models;
using VJN.ModelsDTO.BlogDTOs;

namespace VJN.Services
{
    public interface IBlogService
    {
        public Task<IEnumerable<BlogDTO>> getThreeBlogNews();
        public Task<IEnumerable<BlogDTO>> GetAllBlog();
        public Task<BlogDTO> GetBlogDetail(int id);
    }
}
