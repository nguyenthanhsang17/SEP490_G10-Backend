﻿using AutoMapper;
using VJN.ModelsDTO.BlogDTOs;
using VJN.Repositories;

namespace VJN.Services
{
    public class BlogService : IBlogService
    {
        private readonly IBlogRepository _blogRepository;
        private IMapper _mapper;

        public BlogService(IBlogRepository blogRepository, IMapper mapper)
        {
            _blogRepository = blogRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<BlogDTO>> GetAllBlog()
        {
            var blog = await _blogRepository.GetAllBlog();    
            var blogdto =_mapper.Map<IEnumerable< BlogDTO>>(blog);
            return blogdto;
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
    }
}
