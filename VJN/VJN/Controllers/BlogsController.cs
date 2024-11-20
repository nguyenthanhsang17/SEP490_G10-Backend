using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VJN.Models;
using VJN.ModelsDTO.BlogDTOs;
using VJN.Paging;
using VJN.Services;

namespace VJN.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogsController : ControllerBase
    {
        private readonly IBlogService _blogService;

        public BlogsController(IBlogService blogService)
        {
            _blogService = blogService;
        }

        [HttpGet("GetAllBlog/{pagenumber}")]
        public async Task<ActionResult<PagedResult<BlogDTO>>> GetAllBlog(int pagenumber)
        {
            var blog =  await _blogService.GetAllBlog(pagenumber);
            return Ok(blog);    
        }

        [HttpGet("GetDetailBlog/{id}")]
        public async Task<ActionResult<BlogDTO>> GetDetailBlog(int id)
        {
            var blog = await _blogService.GetBlogDetail(id);
            return Ok(blog);
        }
    }
}
