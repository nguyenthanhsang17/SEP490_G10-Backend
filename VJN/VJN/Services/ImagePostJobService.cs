
using VJN.Repositories;

namespace VJN.Services
{
    public class ImagePostJobService : IImagePostJobService
    {
        public readonly IImagePostJobRepository _imagePostJobRepository;

        public ImagePostJobService(IImagePostJobRepository imagePostJobRepository)
        {
            _imagePostJobRepository = imagePostJobRepository;
        }

        public Task<bool> createImagePostJob(int postid, IEnumerable<int> image)
        {
            var c = _imagePostJobRepository.createImagePostJob(postid, image);
            return c;
        }
    }
}
