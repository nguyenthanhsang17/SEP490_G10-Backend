using Microsoft.EntityFrameworkCore;
using VJN.Models;

namespace VJN.Repositories
{
    public interface IDashBoardRepository
    {
        public Task<int> GetTotalUser();

        public Task<double> GetJobSeekersPercentage();
        public Task<double> GetEmployersPercentage();
        public Task<decimal> GetTotalRevenue();

        public Task<decimal> CalculateMonthlyRevenueAsync(int month, int year);
        public Task<int> GetTotalPackagesSold();
        public Task<int> GetNumberSoldById(int id);
        public Task<decimal> GetRevenueByPackageIdAsync(int id);

        public Task<IEnumerable<ServicePriceList>> GetAllIdPrice();
        public Task<int> GetTotalEmployer();
    }
}
