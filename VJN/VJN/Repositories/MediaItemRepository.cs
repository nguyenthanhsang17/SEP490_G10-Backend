using Microsoft.EntityFrameworkCore;
using VJN.Models;

namespace VJN.Repositories
{
    public class MediaItemRepository : IMediaItemRepository
    {
        private readonly VJNDBContext _context;
        public MediaItemRepository(VJNDBContext context) {
            _context = context;
        }

        public async Task<int> CreateMediaItem(MediaItem mediaItem)
        {
            var mi = await _context.MediaItems.Where(x => x.Url.Equals(mediaItem.Url)).SingleOrDefaultAsync();
            if(mi != null)
            {
                return mi.Id;
            }

            _context.MediaItems.Add(mediaItem);
            await _context.SaveChangesAsync();
            return mediaItem.Id;
        }

    }
}
