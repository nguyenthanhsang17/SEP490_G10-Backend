using Org.BouncyCastle.Crypto;
using System.Xml.Schema;
using VJN.Models;
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

        public async Task<RevenueStatistics> GetRevenueStatistics(DashBoardSearchDTO m)
        {
            var TotalRevenue = await _dashBoardRepository.GetTotalRevenue();
            var lastFiveMonths = new List<MonthsYear>();


            // Bắt đầu từ ngày đầu tiên của tháng đầu tiên
            var currentDate = new DateTime(m.StartDate.Value.Year, m.StartDate.Value.Month, 1);

            // Kết thúc ở cuối tháng cuối cùng
            var endDate = new DateTime(m.EndDate.Value.Year, m.EndDate.Value.Month, 1)
                .AddMonths(1)
                .AddDays(-1);

            while (currentDate <= endDate)
            {
                lastFiveMonths.Add(new MonthsYear
                {
                    Month = currentDate.Month,
                    Year = currentDate.Year
                });
                currentDate = currentDate.AddMonths(1);
            }

            //for (int i = 0; i < 5; i++)
            //{
            //    var date = currentDate.AddMonths(-i);                                                                                                                                                                                                                          
            //    lastFiveMonths.Add(new MonthsYear
            //    {
            //        Month = date.Month,
            //        Year = date.Year
            //    });
            //}
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

        public async Task<PackageStatistics> GetPackageStatistics(DashBoardSearchDTO m)
        {
            var packageStatistics = new PackageStatistics();
            var TotalPackagesSold = await _dashBoardRepository.GetTotalPackagesSold();
            List<PopularPackage> MostPopularPackages = new List<PopularPackage>();

            List<ServicePriceList> ids = (await _dashBoardRepository.GetAllIdPrice()).ToList();

            foreach (var id in ids)
            {
                int NumberSold = await _dashBoardRepository.GetNumberSoldById(id.ServicePriceId, m);
                decimal TotalRevenue =  await _dashBoardRepository.GetRevenueByPackageIdAsync(id.ServicePriceId, m);
                var pp = new PopularPackage()
                {
                    PackageId = id.ServicePriceId,
                    PackageName = "Gói " + id.ServicePriceName,
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

        public async Task<int> GetTotalEmployer()
        {
            var number = await _dashBoardRepository.GetTotalEmployer();
            return number;
        }
    }
}
