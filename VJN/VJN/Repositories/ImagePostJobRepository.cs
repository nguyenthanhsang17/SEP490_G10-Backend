
using Microsoft.EntityFrameworkCore;
using VJN.Models;

namespace VJN.Repositories
{
    public class ImagePostJobRepository : IImagePostJobRepository
    {
        private readonly VJNDBContext _context;
        public ImagePostJobRepository(VJNDBContext context)
        {
            _context = context;
        }
        public async Task<bool> createImagePostJob(int postid, IEnumerable<int> image)
        {
            foreach (var item in image)
            {
                var ImagePostJob = new ImagePostJob
                {
                    PostId = postid,
                    ImageId = item
                };
                _context.ImagePostJobs.Add(ImagePostJob);
                int i= await _context.SaveChangesAsync();
                if(i == 0)
                {
                    return false;
                }
            }
            return true;
        }

        public async Task<IEnumerable<int>> GetImagePostJob(int postid)
        {
            var ids = await _context.ImagePostJobs.Where(ipj => ipj.PostId == postid).Select(ipj=> ipj.ImageId.Value).ToListAsync();
            return ids;
        }

        public async Task<bool> DeleteImagePost(List<int> imageids, int postjobid)
        {
            var imp = await _context.ImagePostJobs.Where(im=>imageids.Contains(im.ImageId.Value)&&im.PostId==postjobid).ToListAsync();
            foreach (var img in imp)
            {
                _context.ImagePostJobs.Remove(img);
                await _context.SaveChangesAsync();
            }
            return true;
        }
    }
}
