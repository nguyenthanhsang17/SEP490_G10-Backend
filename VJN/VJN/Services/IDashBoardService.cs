using VJN.ModelsDTO.DashBoardDTOs;

namespace VJN.Services
{
    public interface IDashBoardService
    {
        public Task<double> GetJobSeekersPercentage();
        public Task<double> GetEmployersPercentage();
        public Task<RevenueStatistics> GetRevenueStatistics();
        public Task<PackageStatistics> GetPackageStatistics();

        public Task<int> GetTotalUser();
    }
}
