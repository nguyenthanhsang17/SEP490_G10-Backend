using Microsoft.EntityFrameworkCore;
using VJN.Models;

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
           var blog =await  _context.Blogs.ToListAsync();
            return blog;
        }

        public async Task<Blog> GetBlogDetail(int id)
        {
            var blog = await _context.Blogs.Include(bl=>bl.ThumbnailNavigation).Where(bl=>bl.BlogId == id).SingleOrDefaultAsync();
            return blog;
        }

        public async Task<IEnumerable<Blog>> getThreeBlogNews()
        {
            var latestBlogs = await _context.Blogs.Include(bl=>bl.ThumbnailNavigation)
            .OrderByDescending(b => b.CreateDate)
            .Take(3)
            .ToListAsync();
            return latestBlogs;
        }
    }
}
