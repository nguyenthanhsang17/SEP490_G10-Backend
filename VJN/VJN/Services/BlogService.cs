using AutoMapper;
using VJN.ModelsDTO.BlogDTOs;
using VJN.Paging;
using VJN.Repositories;

namespace VJN.Services
{
    public class BlogService : IBlogService
    {
        private readonly IBlogRepository _blogRepository;
        private IMapper _mapper;
        int pagesize = 10;

        public BlogService(IBlogRepository blogRepository, IMapper mapper)
        {
            _blogRepository = blogRepository;
            _mapper = mapper;
        }

        public async Task<PagedResult<BlogDTO>> GetAllBlog(int pagenumber)
        {
            var blog = await _blogRepository.GetAllBlog();    
            var blogdto =_mapper.Map<IEnumerable< BlogDTO>>(blog);
            var blg = PaginationHelper.GetPaged<BlogDTO>(blogdto, pagenumber, pagesize);
            return blg;
        }

        public async Task<BlogDTO> GetBlogDetail(int id)
        {
            var blog = await _blogRepository.GetBlogDetail(id);
            var blogdto = _mapper.Map<BlogDTO>(blog);
            return blogdto;
        }

        public async Task<IEnumerable<BlogDTO>> getThreeBlogNews()
        {
            var blog = await _blogRepository.getThreeBlogNews();
            var blogdto = _mapper.Map<IEnumerable<BlogDTO>>(blog);
            return blogdto;
        }

        public async Task<bool> CreateBlog(string title, string description, int thumbnailId, int authorId)
        {
            if (_blogRepository == null)
            {
                return false; 
            }

            return await _blogRepository.CreateBlog(title, description, thumbnailId, authorId);
        }

        public async Task<bool> ChangeStatusBlog(int blogId, int newStatus)
        {
            if (_blogRepository == null)
            {
                return false;
            }
            return await _blogRepository.ChangeStatusBlog(blogId, newStatus);
        }

    }
}
