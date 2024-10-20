using AutoMapper;
using VJN.Models;
using VJN.ModelsDTO.MediaItemDTOs;
using VJN.Repositories;

namespace VJN.Services
{
    public class MediaItemService : IMediaItemService
    {
        private readonly IMediaItemRepository _mediaItemRepository;
        private readonly IMapper _mapper;

        public MediaItemService(IMediaItemRepository mediaItemRepository, IMapper mapper)
        {
            _mediaItemRepository = mediaItemRepository;
            _mapper = mapper;
        }

        public async Task<int> CreateMediaItem(MediaItemDTO mediaItem)
        {
            var media = _mapper.Map<MediaItem>(mediaItem);
            var id = await _mediaItemRepository.CreateMediaItem(media);
            return id;
        }
    }
}
