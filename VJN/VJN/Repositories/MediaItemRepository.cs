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
            _context.MediaItems.Add(mediaItem);
            await _context.SaveChangesAsync();
            return mediaItem.Id;
        }
    }
}
