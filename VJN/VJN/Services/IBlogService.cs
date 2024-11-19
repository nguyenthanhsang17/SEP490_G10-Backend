using VJN.Models;
using VJN.ModelsDTO.BlogDTOs;
using VJN.Paging;

namespace VJN.Services
{
    public interface IBlogService
    {
        public Task<IEnumerable<BlogDTO>> getThreeBlogNews();
        public Task<PagedResult<BlogDTO>> GetAllBlog(int pagenumbe);
        public Task<BlogDTO> GetBlogDetail(int id);
    }
}
