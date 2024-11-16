
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

        public async Task<bool> Addition(int userid, bool check, int time)
        {
            ServicePriceLog servicePriceLog;
            if (check)
            {
                servicePriceLog = await _context.ServicePriceLogs.Where(sp => sp.UserId == userid && sp.NumberPostsUrgentRecruitment > 0).SingleOrDefaultAsync();
            }
            else
            {
                servicePriceLog = await _context.ServicePriceLogs.Where(sp => sp.UserId == userid).SingleOrDefaultAsync();
            }

            if (servicePriceLog != null)
            {
                if (check)
                {
                    servicePriceLog.NumberPostsUrgentRecruitment = servicePriceLog.NumberPostsUrgentRecruitment + time;
                    _context.Entry(servicePriceLog).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    return true;

                }
                else
                {
                    servicePriceLog.NumberPosts = servicePriceLog.NumberPosts + time;
                    _context.Entry(servicePriceLog).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    return true;
                }
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> CheckIsViewAllJobSeeker(int userid)
        {
            throw new NotImplementedException();
        }

        public async Task<int> GetNumberPosts(int userid)
        {
            var sum = await _context.ServicePriceLogs.Where(sp => sp.UserId == userid).SumAsync(sp => sp.NumberPosts);
            return sum.Value;
        }

        public async Task<bool> isexpired(int userid)
        {
            var oneMonthAgo = DateTime.Now.AddMonths(-1);
            var recentLogs = await _context.ServicePriceLogs
                .Where(log => log.UserId == userid && log.RegisterDate >= oneMonthAgo && log.NumberPosts > 0 && log.NumberPostsUrgentRecruitment > 0)
                .AnyAsync();
            return recentLogs;
        }

        public async Task<int> NumberPostsUrgentRecruitment(int userid)
        {
            var sum = await _context.ServicePriceLogs.Where(sp => sp.UserId == userid).SumAsync(sp => sp.NumberPostsUrgentRecruitment);
            return sum.Value;
        }

        public async Task<bool> subtraction(int userid, bool check, int time)
        {
            //check = true la dang bai dạc biet
            //check = false dang bai thuong
            ServicePriceLog servicePriceLog;
            if (check)
            {
                servicePriceLog = await _context.ServicePriceLogs.Where(sp => sp.UserId == userid && sp.NumberPostsUrgentRecruitment > 0).SingleOrDefaultAsync();
            }
            else
            {
                servicePriceLog = await _context.ServicePriceLogs.Where(sp => sp.UserId == userid).SingleOrDefaultAsync();
            }

            if (servicePriceLog != null)
            {
                if (check)
                {
                    if (servicePriceLog.NumberPostsUrgentRecruitment >= time )
                    {
                        servicePriceLog.NumberPostsUrgentRecruitment = servicePriceLog.NumberPostsUrgentRecruitment - time;
                        _context.Entry(servicePriceLog).State = EntityState.Modified;
                        await _context.SaveChangesAsync();
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    if (servicePriceLog.NumberPosts >= time)
                    {
                        servicePriceLog.NumberPosts = servicePriceLog.NumberPosts - time;
                        _context.Entry(servicePriceLog).State = EntityState.Modified;
                        await _context.SaveChangesAsync();
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            else
            {
                return false;
            }

        }
    }
}
