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
