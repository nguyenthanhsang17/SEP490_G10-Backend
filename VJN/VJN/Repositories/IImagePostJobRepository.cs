namespace VJN.Repositories
{
    public interface IImagePostJobRepository
    {
        public Task<bool> createImagePostJob(int postid, IEnumerable<int> image);
        public Task<IEnumerable<int>> GetImagePostJob(int postid);
        public Task<bool> DeleteImagePost(List<int> imageids, int postjobid);
    }
}
