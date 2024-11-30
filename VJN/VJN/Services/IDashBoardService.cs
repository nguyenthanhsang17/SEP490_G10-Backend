using VJN.ModelsDTO.DashBoardDTOs;

namespace VJN.Services
{
    public interface IDashBoardService
    {
        public Task<double> GetJobSeekersPercentage();
        public Task<double> GetEmployersPercentage();
        public Task<RevenueStatistics> GetRevenueStatistics(DashBoardSearchDTO m);
        public Task<PackageStatistics> GetPackageStatistics(DashBoardSearchDTO m);

        public Task<int> GetTotalUser();

        public Task<int> GetTotalEmployer();
    }
}
