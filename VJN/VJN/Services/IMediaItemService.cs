using VJN.Models;
using VJN.ModelsDTO.MediaItemDTOs;

namespace VJN.Services
{
    public interface IMediaItemService
    {
        public Task<int> CreateMediaItem(MediaItemDTO mediaItem);
    }
}
