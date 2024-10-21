using Microsoft.EntityFrameworkCore;
using VJN.Models;

namespace VJN.Repositories
{
    public class PostJobRepository : IPostJobRepository
    {
        private readonly VJNDBContext _context;

        public PostJobRepository(VJNDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PostJob>> GetPorpularJob()
        {
            var jobTopIds = await _context.PostJobs.GroupBy(pj => pj.PostId).Select(g => new
            {
                PostId = g.Key,
                ApplicantCount = _context.ApplyJobs.Count(aj => aj.PostId == g.Key)
            })
            .OrderByDescending(x => x.ApplicantCount)
            .Take(3)
            .Select(x => x.PostId)
            .ToListAsync();

            var topPosts = await _context.PostJobs.Include(pj=>pj.Author).Include(j => j.JobCategory).Include(j => j.SalaryTypes)
                .Where(pj => jobTopIds.Contains(pj.PostId))
                .ToListAsync();

            return topPosts;
        }

        public async Task<string> getThumnailJob(int id)
        {
            var urls = await _context.ImagePostJobs.Where(im => im.PostId == id).ToListAsync();
            var idurl = urls.FirstOrDefault().ImageId;

            var image = await _context.MediaItems.Where(mi=>mi.Id==id).SingleOrDefaultAsync();
            return image.Url;
        }
    }
}
