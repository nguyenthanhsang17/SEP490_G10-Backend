
using Microsoft.EntityFrameworkCore;
using VJN.Models;
using VJN.ModelsDTO.ServiceDTOs;

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
            Service service = null;
            if (check)
            {
                service = await _context.Services.Where(sp => sp.UserId == userid && sp.NumberPostsUrgentRecruitment > 0).SingleOrDefaultAsync();
            }
            else
            {
                service = await _context.Services.Where(sp => sp.UserId == userid).SingleOrDefaultAsync();
            }

            if (service != null)
            {
                if (check)
                {
                    service.NumberPostsUrgentRecruitment = service.NumberPostsUrgentRecruitment + time;
                    _context.Entry(service).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    return true;

                }
                else
                {
                    service.NumberPosts = service.NumberPosts + time;
                    _context.Entry(service).State = EntityState.Modified;
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
            DateTime currentDate = DateTime.Now; // Hoặc DateTime.UtcNow nếu cần múi giờ chuẩn
            var check = await _context.Services
                .Where(sv => sv.UserId == userid
                          && sv.IsFindJobseekers == 1
                          && sv.ExpirationDate.HasValue // Nếu cột ExpirationDate cho phép null
                          && sv.ExpirationDate >= currentDate)
                .AnyAsync();
            return check;
        }

        public async Task<int> GetNumberPosts(int userid)
        {
            var sum = await _context.Services.Where(sp => sp.UserId == userid).SumAsync(sp => sp.NumberPosts);
            return sum.Value;
        }

        public async Task<IEnumerable<ServicePriceLog>> GetPaymentHistory(int userid)
        {
            var register = await _context.ServicePriceLogs.Where(sp => sp.UserId == userid).Include(sp=>sp.ServicePrice).ToListAsync();
            return register;
        }

        public async Task<IEnumerable<ServicePriceLog>> GetAllPaymentHistory()
        {
            var register = await _context.ServicePriceLogs.ToListAsync();
            return register;
        }

        public async Task<int> NumberPostsUrgentRecruitment(int userid)
        {
            var sum = await _context.Services.Where(sp => sp.UserId == userid).SumAsync(sp => sp.NumberPostsUrgentRecruitment);
            return sum.Value;
        }

        public async Task<bool> subtraction(int userid, bool check, int time)
        {
            //check = true la dang bai dạc biet
            //check = false dang bai thuong
            Service servicePriceLog;
            if (check)
            {
                servicePriceLog = await _context.Services.Where(sp => sp.UserId == userid && sp.NumberPostsUrgentRecruitment > 0).SingleOrDefaultAsync();
            }
            else
            {
                servicePriceLog = await _context.Services.Where(sp => sp.UserId == userid).SingleOrDefaultAsync();
            }

            if (servicePriceLog != null)
            {
                if (check)
                {
                    if (servicePriceLog.NumberPostsUrgentRecruitment >= time)
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

        public async Task<bool> CreateServicePriceLog(ServicePriceLog model)
        {
            _context.ServicePriceLogs.Add(model);
            int a = await _context.SaveChangesAsync();
            if (a > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> AddService(int userid, int ServicePriceID)
        {
            var ServicePrice = await _context.ServicePriceLists.Where(sp => sp.ServicePriceId == ServicePriceID).SingleOrDefaultAsync();

            int NumberPosts = ServicePrice.NumberPosts ?? 0;
            int NumberPostsUrgentRecruitment = ServicePrice.NumberPostsUrgentRecruitment ?? 0;
            int IsFindJobseekers = ServicePrice.IsFindJobseekers ?? 0;
            int DurationsMonth = ServicePrice.DurationsMonth ?? 0;
            DateTime currentDate = DateTime.Now;
            var userService = await _context.Services.Where(us => us.UserId == userid).SingleOrDefaultAsync();
            if (userService != null)
            {
                userService.NumberPosts = userService.NumberPosts + NumberPosts;
                userService.NumberPostsUrgentRecruitment = userService.NumberPostsUrgentRecruitment + NumberPostsUrgentRecruitment;

                if (IsFindJobseekers == 1)
                {
                    if (userService.IsFindJobseekers == 0)
                    {
                        userService.ExpirationDate = currentDate.AddMonths(DurationsMonth);
                        userService.IsFindJobseekers = IsFindJobseekers;
                    }
                    else
                    {
                        userService.IsFindJobseekers = IsFindJobseekers;
                        if (currentDate > userService.ExpirationDate)
                        {
                            userService.ExpirationDate = currentDate.AddMonths(DurationsMonth);
                        }
                        else
                        {
                            userService.ExpirationDate = userService.ExpirationDate.Value.AddMonths(DurationsMonth);
                        }
                    }
                }
                _context.Entry(userService).State = EntityState.Modified;
                int i= await _context.SaveChangesAsync();
                return i > 0;
            }
            else
            {
                var service = new Service()
                {
                    UserId = userid,
                    NumberPosts = NumberPosts,
                    NumberPostsUrgentRecruitment = NumberPostsUrgentRecruitment,
                    IsFindJobseekers = IsFindJobseekers,
                    ExpirationDate = currentDate.AddMonths(DurationsMonth),
                };
                _context.Services.Add(service);
                int i = await _context.SaveChangesAsync();
                if (i > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

        }

        public async Task<Service> GetAllServiceByUserId(int userid)
        {
            var service  = await _context.Services.Where(sv=>sv.UserId==userid).SingleOrDefaultAsync();
            return service;
        }
    }
}
