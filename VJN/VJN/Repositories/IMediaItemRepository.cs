using VJN.Models;

namespace VJN.Repositories
{
    public interface IMediaItemRepository
    {
        public Task<int> CreateMediaItem(MediaItem mediaItem);
    }
}
