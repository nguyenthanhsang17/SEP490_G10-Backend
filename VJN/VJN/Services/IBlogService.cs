using VJN.ModelsDTO.BlogDTOs;

namespace VJN.Services
{
    public interface IBlogService
    {
        public Task<IEnumerable<BlogDTO>> getThreeBlogNews();
    }
}
