namespace VJN.Repositories
{
    public interface IImagePostJobRepository
    {
        public Task<bool> createImagePostJob(int postid, IEnumerable<int> image);
    }
}
