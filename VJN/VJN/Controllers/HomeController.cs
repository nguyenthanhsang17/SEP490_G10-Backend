using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VJN.Models;
using VJN.ModelsDTO.BlogDTOs;
using VJN.Repositories;
using VJN.Services;

namespace VJN.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IBlogService _blogService;
        private readonly IPostJobService _postJobService;   
        private readonly IPostJobRepository _postJobRepository;

        public HomeController(IBlogService blogService, IPostJobService postJobService, IPostJobRepository postJobRepository)
        {
            _blogService = blogService;
            _postJobService = postJobService;
            _postJobRepository = postJobRepository;
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

        [HttpGet("getSang")]
        public async Task<IActionResult> getSang([FromQuery] int i)
        {
            var pdto = await _postJobRepository.getThumnailJob(i);
            return Ok(pdto);
        }
    }
}
