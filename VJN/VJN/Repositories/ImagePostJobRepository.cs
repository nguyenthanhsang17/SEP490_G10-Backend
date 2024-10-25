
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
    }
}
