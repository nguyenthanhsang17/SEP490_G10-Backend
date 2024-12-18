﻿
using Microsoft.EntityFrameworkCore;
using VJN.Models;
using VJN.ModelsDTO.DashBoardDTOs;

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

        public async Task<IEnumerable<ServicePriceList>> GetAllIdPrice()
        {
            var ids = await _context.ServicePriceLists.ToListAsync();
            return ids;
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

        public async Task<int> GetNumberSoldById(int id, DashBoardSearchDTO m)
        {
            var currentDate = new DateTime(m.StartDate.Value.Year, m.StartDate.Value.Month, 1);

            // Kết thúc ở cuối tháng cuối cùng
            var endDate = new DateTime(m.EndDate.Value.Year, m.EndDate.Value.Month, 1)
                .AddMonths(1)
                .AddDays(-1);

            var salesCount = await _context.ServicePriceLogs
            .Where(log => log.ServicePriceId == id && log.RegisterDate >= currentDate && log.RegisterDate <= endDate)
            .CountAsync();

            return salesCount;
        }

        public async Task<decimal> GetRevenueByPackageIdAsync(int id, DashBoardSearchDTO m)
        {

            var price = await _context.ServicePriceLists
                .Where(p => p.ServicePriceId == id)
                .Select(p => p.Price)
                .FirstOrDefaultAsync();

            if (price == 0) return 0;

            var currentDate = new DateTime(m.StartDate.Value.Year, m.StartDate.Value.Month, 1);

            // Kết thúc ở cuối tháng cuối cùng
            var endDate = new DateTime(m.EndDate.Value.Year, m.EndDate.Value.Month, 1)
                .AddMonths(1)
                .AddDays(-1);

            var salesCount = await _context.ServicePriceLogs
                .Where(log => log.ServicePriceId == id && log.RegisterDate >= currentDate && log.RegisterDate <= endDate)
                .CountAsync();
            var result = salesCount * price;
            return result.Value;
        }

        public async Task<int> GetTotalPackagesSold()
        {
            int currentYear = DateTime.Now.Year;

            // Ngày đầu tiên và ngày cuối cùng của năm hiện tại
            DateTime startOfYear = new DateTime(currentYear, 1, 1);
            DateTime endOfYear = new DateTime(currentYear, 12, 31, 23, 59, 59);

            var number = await _context.ServicePriceLogs.Where(log => log.RegisterDate.HasValue &&
                      log.RegisterDate.Value >= startOfYear &&
                      log.RegisterDate.Value <= endOfYear).CountAsync();
            return number;
        }

        public async Task<decimal> GetTotalRevenue()
        {
            int currentYear = DateTime.Now.Year;

            // Ngày đầu tiên và ngày cuối cùng của năm hiện tại
            DateTime startOfYear = new DateTime(currentYear, 1, 1);
            DateTime endOfYear = new DateTime(currentYear, 12, 31, 23, 59, 59);

            var totalRevenue = await _context.ServicePriceLogs
                .Where(log => log.RegisterDate.HasValue &&
                      log.RegisterDate.Value >= startOfYear &&
                      log.RegisterDate.Value <= endOfYear)
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

        public async Task<int> GetTotalEmployer()
        {
            var totalUsers = await _context.Users.CountAsync();

            var usersWithPostJob = await _context.Users
                .Where(user => _context.PostJobs.Any(post => post.AuthorId == user.UserId))
                .CountAsync();

            return usersWithPostJob;
        }





    }
}
