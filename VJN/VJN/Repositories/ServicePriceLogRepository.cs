
using Microsoft.EntityFrameworkCore;
using VJN.Models;

namespace VJN.Repositories
{
    public class ServicePriceLogRepository : IServicePriceLogRepository
    {
        private readonly VJNDBContext _context;
        public ServicePriceLogRepository(VJNDBContext context)
        {
            _context = context;
        }
        public async Task<bool> isexpired(int userid)
        {
            var oneMonthAgo = DateTime.Now.AddMonths(-1);
            var recentLogs = await _context.ServicePriceLogs
                .Where(log => log.UserId == userid && log.RegisterDate >= oneMonthAgo && log.NumberPostRemaining>0&&log.NumberPostsUrgentRecruitmentRemaining>0)
                .AnyAsync();
            return recentLogs;
        }

        public async Task<bool> subtraction(int userid, bool check)
        {
            //check = true  dang bai tuyenr gap
            //check = false dang bai binh thuong
            var oneMonthAgo = DateTime.Now.AddMonths(-1);
            var recentLogs = _context.ServicePriceLogs
                .Where(log => log.UserId == userid && log.RegisterDate >= oneMonthAgo);
            if(!check)
            {
                recentLogs = recentLogs.Where(log => log.NumberPostRemaining > 0);
            }
            else
            {
                recentLogs = recentLogs.Where(log => log.NumberPostsUrgentRecruitmentRemaining > 0);
            }

            var exitst = await recentLogs.AnyAsync();
            if(exitst)
            {
                var log = await recentLogs.FirstOrDefaultAsync();
                if (!check)
                {
                    log.NumberPostRemaining = log.NumberPostRemaining - 1;
                }
                else
                {
                    log.NumberPostsUrgentRecruitmentRemaining = log.NumberPostsUrgentRecruitmentRemaining - 1;
                }
                _context.Entry(log).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }

        }
    }
}
