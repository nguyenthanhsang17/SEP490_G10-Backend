namespace VJN.Services
{
    public interface IImagePostJobService
    {
        public Task<bool> createImagePostJob(int postid, IEnumerable<int> image);
    }
}
