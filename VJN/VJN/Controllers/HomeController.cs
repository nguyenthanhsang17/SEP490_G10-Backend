using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VJN.Models;
using VJN.ModelsDTO.BlogDTOs;
using VJN.Services;

namespace VJN.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IBlogService _blogService;
        private readonly IPostJobService _postJobService;   

        public HomeController(IBlogService blogService, IPostJobService postJobService)
        {
            _blogService = blogService;
            _postJobService = postJobService;
        }

        [HttpGet("getThreeBlogNews")]
        public async Task<ActionResult<IEnumerable<BlogDTO>>> getThreeBlogNews()
        {
            var blogdto = await _blogService.getThreeBlogNews();
            return Ok(blogdto);
        }

        [HttpGet("getPopularJob")]
        public async Task<ActionResult<IEnumerable<PostJob>>> getPopularJob()
        {
            var pdto = await _postJobService.getPorpularJob();
            return Ok(pdto);
        }
    }
}
