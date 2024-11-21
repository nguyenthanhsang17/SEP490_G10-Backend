using VJN.ModelsDTO.BlogDTOs;

namespace VJN.Services
{
    public interface IBlogService
    {
        public Task<IEnumerable<BlogDTO>> getThreeBlogNews();
        public  Task<bool> CreateBlog(string title, string description, int thumbnailId, int authorId);
        public Task<bool> ChangeStatusBlog(int blogId, int newStatus);
    }
}
