using VJN.Models;

namespace VJN.Repositories
{
    public interface IReportRepository
    {
        public Task<IEnumerable<Report>> getAllReportById(int postId);
    }
}
