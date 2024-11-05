namespace VJN.Repositories
{
    public interface IReportMediaRepository
    {
        public Task<bool> CreateReportMedia(int reportid, List<int> images);
    }
}
