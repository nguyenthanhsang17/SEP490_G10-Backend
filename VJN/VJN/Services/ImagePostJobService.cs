
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

        public async Task<bool> DeleteImagePost(List<int> imageids, int postjobid)
        {
            var c = await _imagePostJobRepository.DeleteImagePost(imageids, postjobid);
            return c;
        }

        public async Task<IEnumerable<int>> GetImagePostJob(int postid)
        {
            var c  = await _imagePostJobRepository.GetImagePostJob(postid);
            return c;
        }
    }
}
