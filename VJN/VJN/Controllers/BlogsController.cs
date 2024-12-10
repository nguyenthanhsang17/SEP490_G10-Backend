using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Imagekit.Sdk;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VJN.Models;
using VJN.ModelsDTO.BlogDTOs;
using VJN.ModelsDTO.MediaItemDTOs;
using VJN.Paging;
using VJN.Services;

namespace VJN.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogsController : ControllerBase
    {
        private readonly VJNDBContext _context;
        private readonly IMediaItemService _mediaItemService;
        private readonly ImagekitClient _imagekitClient;
        private readonly IBlogService _blogService;
        public BlogsController(VJNDBContext context, IMediaItemService mediaItemService, IBlogService blogService, ImagekitClient imagekitClient)
        {
            _context = context;
            _mediaItemService = mediaItemService;
            _blogService = blogService;
            _imagekitClient = imagekitClient;
        }

        // GET: api/Blogs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BlogDTO>>> GetAllBlogs()
        {
            if (_context.Blogs == null)
            {
                return NotFound();
            }

            var blogs = await _context.Blogs
                        .Include(b => b.Author)
                        .Include(b => b.ThumbnailNavigation)
                        .ToListAsync();

            var blogDTOs = blogs.Select(blog => new BlogDTO
            {
                BlogId = blog.BlogId,
                BlogTitle = blog.BlogTitle,
                BlogDescription = blog.BlogDescription,
                CreateDate = blog.CreateDate,
                AuthorName = blog.Author.FullName,
                Thumbnail = blog.ThumbnailNavigation.Url,
                Status = blog.Status,
            }).ToList();

            return Ok(blogDTOs);
        }

        [HttpGet("GetAllBlog")]
        public async Task<ActionResult<PagedResult<BlogDTO>>> GetAllBlogs(int pageNumber = 1, int pageSize = 10, string? title = "", int? status = null, string sortOrder = "desc")
        {
            if (_context.Blogs == null)
            {
                return NotFound();
            }

            var query = _context.Blogs
                .Include(b => b.Author)
                .Include(b => b.ThumbnailNavigation)
                .AsQueryable();

            if (!string.IsNullOrEmpty(title))
            {
                query = query.Where(b => b.BlogTitle.Contains(title));
            }

            if (status.HasValue)
            {
                query = query.Where(b => b.Status == status.Value);
            }

            if (sortOrder == "asc")
            {
                query = query.OrderBy(b => b.CreateDate);
            }
            else
            {
                query = query.OrderByDescending(b => b.CreateDate);
            }

            var totalCount = await query.CountAsync();

            var blogs = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var blogDTOs = blogs.Select(blog => new BlogDTO
            {
                BlogId = blog.BlogId,
                BlogTitle = blog.BlogTitle,
                BlogDescription = blog.BlogDescription,
                CreateDate = blog.CreateDate,
                AuthorName = blog.Author.FullName,
                Thumbnail = blog.ThumbnailNavigation.Url,
                Status = blog.Status,
            }).ToList();

            var pagedResult = new PagedResult<BlogDTO>(blogDTOs, totalCount, pageNumber, pageSize);

            return Ok(pagedResult);
        }

        [HttpGet("GetAllBlog/{pagenumber}")]
        public async Task<ActionResult<PagedResult<BlogDTO>>> GetAllBlog(int pagenumber)
        {
            var blog = await _blogService.GetAllBlog(pagenumber);
            return Ok(blog);
        }



        // GET: api/Blogs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BlogDTO>> GetBlog(int id)
        {
            if (_context.Blogs == null)
            {
                return NotFound();
            }
            var blog = await _context.Blogs.
                       Include(b => b.Author).
                       Include(b => b.ThumbnailNavigation).
                       FirstOrDefaultAsync(b => b.BlogId == id);

            if (blog == null)
            {
                return NotFound();
            }

            return new BlogDTO
            {
                BlogId = blog.BlogId,
                BlogTitle = blog.BlogTitle,
                BlogDescription = blog.BlogDescription,
                CreateDate = blog.CreateDate,
                AuthorName = blog.Author.FullName,
                Thumbnail = blog.ThumbnailNavigation.Url,
                Status = blog.Status
            };
        }

        [HttpPost("createblog")]
        public async Task<IActionResult> PutBlog([FromForm] BlogUpdateDTO blog)
        {
            if (blog == null)
            {
                return BadRequest(new { Message = "Không được để trống, người dùng cần nhập đầy đủ" });
            }
            else
            {
                var userId = GetUserIdFromToken();
                var thumbnailid = 0;
                if (blog.Thumbnail == null || blog.Thumbnail.Length == 0)
                {
                    return BadRequest(new { Message = "Blog Phải có ảnh" });
                }
                else
                {
                    using (var memoryStream = new System.IO.MemoryStream())
                    {
                        await blog.Thumbnail.CopyToAsync(memoryStream);
                        byte[] fileBytes = memoryStream.ToArray();
                        Console.WriteLine("day la file name: " + blog.Thumbnail.FileName);
                        try
                        {
                            FileCreateRequest uploadRequest = new FileCreateRequest
                            {
                                file = fileBytes,
                                fileName = blog.Thumbnail.FileName,
                                overwriteFile = true
                            };
                            Result result = _imagekitClient.Upload(uploadRequest);
                            var media = new MediaItemDTO
                            {
                                Url = result.url,
                                Status = true
                            };
                            Console.WriteLine(Url);
                            thumbnailid = await _mediaItemService.CreateMediaItem(media);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
                var re = await _blogService.CreateBlog(blog.BlogTitle, blog.BlogDescription, thumbnailid, int.Parse(userId));
                if (re)
                {
                    return Ok(new { Message = "Tạo bài viết thành công" });
                }
                else
                {
                    return BadRequest(new { Message = "Tạo bài viết thất bại" });
                }
            }
        }

        private string GetUserIdFromToken()
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            Console.WriteLine(token);
            if (string.IsNullOrEmpty(token))
            {
                throw new Exception("Missing token in Authorization header.");
            }

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var userIdClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "nameid");

            if (userIdClaim == null)
            {
                throw new Exception("User ID not found in token.");
            }

            return userIdClaim.Value;
        }
        [HttpGet("GetDetailBlog/{id}")]
        public async Task<ActionResult<BlogDTO>> GetDetailBlog(int id)
        {
            var blog = await _blogService.GetBlogDetail(id);
            return Ok(blog);
        }

        [HttpPut("show")]
        public async Task<IActionResult> ShowBlog(int blogId)
        {
            int status = 0;
            var re = await _blogService.ChangeStatusBlog(blogId, status);
            if (re)
            {
                return Ok(new { Message = "hiển thị bài viết thành công" });
            }
            else
            {
                return BadRequest(new { Message = "hiển thị bài viết thất bại" });
            }
        }
        [HttpPut("hide")]
        public async Task<IActionResult> HideBlog(int blogId)
        {
            int status = 1;
            var re = await _blogService.ChangeStatusBlog(blogId, status);
            if (re)
            {
                return Ok(new { Message = "ẩn bài viết thành công" });
            }
            else
            {
                return BadRequest(new { Message = "ẩn bài viết thất bại" });
            }
        }

    }
}
