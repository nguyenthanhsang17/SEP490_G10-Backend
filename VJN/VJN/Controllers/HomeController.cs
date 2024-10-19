using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VJN.ModelsDTO.BlogDTOs;
using VJN.Services;

namespace VJN.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IBlogService _blogService;

        public HomeController(IBlogService blogService)
        {
            _blogService = blogService;
        }

        [HttpGet("getThreeBlogNews")]
        public async Task<ActionResult<IEnumerable<BlogDTO>>> getThreeBlogNews()
        {
            var blogdto = await _blogService.getThreeBlogNews();
            return Ok(blogdto);
        }
    }
}
