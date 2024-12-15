using MessagePack.Formatters;
using Microsoft.EntityFrameworkCore;
using VJN.Models;
using VJN.ModelsDTO.BlogDTOs;

namespace VJN.Repositories
{
    public class BlogRepository : IBlogRepository
    {
        private readonly VJNDBContext _context;
        public BlogRepository(VJNDBContext context) {
            _context = context;
        }

        public async Task<IEnumerable<Blog>> GetAllBlog()
        {
            var blog = await _context.Blogs.Include(bl => bl.ThumbnailNavigation).Include(bl => bl.Author).Where(bl=>bl.Status==1).ToListAsync();
            return blog;
        }

        public async Task<Blog> GetBlogDetail(int id)
        {
            var blog = await _context.Blogs.Include(bl=>bl.ThumbnailNavigation).Include(bl => bl.Author).Where(bl=>bl.BlogId == id).SingleOrDefaultAsync();
            return blog;
        }

        public async Task<IEnumerable<Blog>> getThreeBlogNews()
        {
            var latestBlogs = await _context.Blogs.Where(bl=>bl.Status==1).Include(bl=>bl.ThumbnailNavigation)
            .OrderByDescending(b => b.CreateDate)
            .Take(3)
            .ToListAsync();
            return latestBlogs;
        }

        public async Task<bool> CreateBlog(string title, string description, int thumbnailId, int authorId)
        {
            try
            {
                Blog newBlog = new Blog
                {
                    BlogTitle = title,
                    BlogDescription = description,
                    Thumbnail = thumbnailId,
                    AuthorId = authorId,
                    Status = 0, 
                    CreateDate = DateTime.Now 
                };

                _context.Blogs.Add(newBlog);
                await _context.SaveChangesAsync();

                return true; 
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating blog: {ex.Message}");

                return false; 
            }
        }

        public async Task<bool> ChangeStatusBlog(int blogId, int newStatus)
        {
            // Tìm blog theo ID
            var blog = await _context.Blogs.FindAsync(blogId);

            // Kiểm tra nếu blog không tồn tại
            if (blog == null)
            {
                return false;
            }
            blog.Status = newStatus;
            _context.Blogs.Update(blog);
            await _context.SaveChangesAsync();
            return true;
        }


    }
}
