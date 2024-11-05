namespace VJN.Services
{
    public interface IReportMediaServices
    {
        public Task<bool> CreateReportMedia(int reportid, List<int> images);
    }
}
