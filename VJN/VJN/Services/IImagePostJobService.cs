namespace VJN.Services
{
    public interface IImagePostJobService
    {
        public Task<bool> createImagePostJob(int postid, IEnumerable<int> image);
        public Task<IEnumerable<int>> GetImagePostJob(int postid);
        public Task<bool> DeleteImagePost(List<int> imageids, int postjobid);
    }
}
