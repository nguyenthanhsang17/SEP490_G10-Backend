using System.Xml.Schema;
using VJN.ModelsDTO.DashBoardDTOs;
using VJN.Repositories;

namespace VJN.Services
{
    public class DashBoardService : IDashBoardService
    {
        private readonly IDashBoardRepository _dashBoardRepository;

        public DashBoardService(IDashBoardRepository dashBoardRepository)
        {
            _dashBoardRepository = dashBoardRepository;
        }

        public async Task<double> GetEmployersPercentage()
        {
            var percent = await _dashBoardRepository.GetEmployersPercentage();
            return percent;
        }

        public async Task<double> GetJobSeekersPercentage()
        {
            var percent = await _dashBoardRepository.GetJobSeekersPercentage();
            return percent;
        }

        public async Task<RevenueStatistics> GetRevenueStatistics()
        {
            var TotalRevenue = await _dashBoardRepository.GetTotalRevenue();
            var currentDate = DateTime.Now;
            var lastFiveMonths = new List<MonthsYear>();

            for (int i = 0; i < 5; i++)
            {
                var date = currentDate.AddMonths(-i);
                lastFiveMonths.Add(new MonthsYear
                {
                    Month = date.Month,
                    Year = date.Year
                });
            }
            List<MonthlyRevenue> MonthlyRevenue = new List<MonthlyRevenue>();

            foreach (var date in lastFiveMonths)
            {
                string month = date.Year.ToString()+"-"+ date.Month.ToString();
                decimal Revenue = await _dashBoardRepository.CalculateMonthlyRevenueAsync(date.Month, date.Year);
                MonthlyRevenue.Add(new MonthlyRevenue
                {
                    Month = month,
                    Revenue = Revenue
                });
            }
            var result = new RevenueStatistics
            {
                TotalRevenue = TotalRevenue,
                MonthlyRevenue = MonthlyRevenue
            };
            return result;
        }

        public async Task<PackageStatistics> GetPackageStatistics()
        {
            var packageStatistics = new PackageStatistics();
            var TotalPackagesSold = await _dashBoardRepository.GetTotalPackagesSold();
            List<PopularPackage> MostPopularPackages = new List<PopularPackage>();

            List<int> ids = (await _dashBoardRepository.GetAllIdPrice()).ToList();

            foreach (var id in ids)
            {
                int NumberSold = await _dashBoardRepository.GetNumberSoldById(id);
                decimal TotalRevenue =  await _dashBoardRepository.GetRevenueByPackageIdAsync(id);
                var pp = new PopularPackage()
                {
                    PackageId = id,
                    PackageName = "Gói " + id,
                    NumberSold = NumberSold,
                    TotalRevenue = TotalRevenue,
                };
                MostPopularPackages.Add(pp);
            }

            packageStatistics.TotalPackagesSold = TotalPackagesSold;
            packageStatistics.MostPopularPackages = MostPopularPackages;
            return packageStatistics;
        }

        public async Task<int> GetTotalUser()
        {
            var TotalUser = await _dashBoardRepository.GetTotalUser();
            return TotalUser;
        }
    }
}
