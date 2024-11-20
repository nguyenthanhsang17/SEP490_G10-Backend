
using Microsoft.EntityFrameworkCore;
using VJN.Models;

namespace VJN.Repositories
{
    public class DashBoardRepository : IDashBoardRepository
    {
        private readonly VJNDBContext _context;

        public DashBoardRepository(VJNDBContext context)
        {
            _context = context;
        }

        public async Task<decimal> CalculateMonthlyRevenueAsync(int month, int year)
        {
            var startDate = new DateTime(year, month, 1);
            var endDate = startDate.AddMonths(1);

            var revenue = await _context.ServicePriceLogs
                .Where(log => log.RegisterDate >= startDate && log.RegisterDate < endDate)
                .Join(_context.ServicePriceLists,
                      log => log.ServicePriceId,
                      price => price.ServicePriceId,
                      (log, price) => price.Price)
                .SumAsync(price => price);
            return revenue.Value;
        }

        public async Task<IEnumerable<int>> GetAllIdPrice()
        {
            var ids  = await _context.ServicePriceLists.ToListAsync();
            var idss = ids.Select(spl => spl.ServicePriceId).ToList();
            return idss;
        }

        public async Task<double> GetEmployersPercentage()
        {
            var totalUsers = await _context.Users.CountAsync();

            var usersWithPostJob = await _context.Users
                .Where(user => _context.PostJobs.Any(post => post.AuthorId == user.UserId))
                .CountAsync();

            if (totalUsers == 0) return 0;

            return (double)usersWithPostJob / totalUsers * 100;
        }

        public async Task<double> GetJobSeekersPercentage()
        {
            var totalUsers = await _context.Users.CountAsync();

            var jobSeekersCount = await _context.Users
                .Where(user => !_context.PostJobs.Any(post => post.AuthorId == user.UserId))
                .CountAsync();

            if (totalUsers == 0) return 0;

            return (double)jobSeekersCount / totalUsers * 100;
        }

        public async Task<int> GetNumberSoldById(int id)
        {
            var salesCount = await _context.ServicePriceLogs
            .Where(log => log.ServicePriceId == id)
            .CountAsync();

            return salesCount;
        }

        public async Task<decimal> GetRevenueByPackageIdAsync(int id)
        {

            var price = await _context.ServicePriceLists
                .Where(p => p.ServicePriceId == id)
                .Select(p => p.Price)
                .FirstOrDefaultAsync();

            if (price == 0) return 0;

            var salesCount = await _context.ServicePriceLogs
                .Where(log => log.ServicePriceId == id)
                .CountAsync();
            var result = salesCount * price;
            return result.Value;
        }

        public async Task<int> GetTotalPackagesSold()
        {
            var number = await _context.ServicePriceLogs.CountAsync();
            return number;
        }

        public async Task<decimal> GetTotalRevenue()
        {
            var totalRevenue = await _context.ServicePriceLogs
                .Join(_context.ServicePriceLists,
                    log => log.ServicePriceId,
                    price => price.ServicePriceId,
                    (log, price) => price.Price
                )
                .SumAsync(price => price);
            return totalRevenue.Value;
        }

        public async Task<int> GetTotalUser()
        {
            var i = await _context.Users.CountAsync();
            return i;
        }





    }
}
