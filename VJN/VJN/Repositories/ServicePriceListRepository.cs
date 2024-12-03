using Microsoft.EntityFrameworkCore;
using VJN.Models;

namespace VJN.Repositories
{
    public class ServicePriceListRepository : IServicePriceListRepository
    {

        private readonly VJNDBContext _context;

        public ServicePriceListRepository(VJNDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ServicePriceList>> GetAllServicePriceList()
        {
            var prices  = await _context.ServicePriceLists.ToListAsync();
            return prices;
        }

        public async Task<ServicePriceList> GetServicePriceList(int servicePriceListId)
        {
            var spl = await _context.ServicePriceLists.Where(spl=>spl.ServicePriceId==servicePriceListId).SingleOrDefaultAsync();
            return spl;
        }

        public async Task<ServicePriceList> CreateServicePriceList(ServicePriceList newServicePriceList)
        {
            await _context.ServicePriceLists.AddAsync(newServicePriceList);
            await _context.SaveChangesAsync();
            return newServicePriceList;
        }

        public async Task<bool> ChangeStatusPriceList(int id, int newStatus)
        {
            var servicePriceList = await _context.ServicePriceLists.FindAsync(id);
            if (servicePriceList == null)
            {
                return false;
            }
            servicePriceList.Status = newStatus;
            _context.ServicePriceLists.Update(servicePriceList);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<ServicePriceList>> GetAllServicePriceListWithStatus1()
        {
            var spl = await _context.ServicePriceLists.Where(spl=>spl.Status==1).ToListAsync();
            return spl;
        }

        public async Task<int> RemoveServicePricedList(int id)
        {
            var check = await _context.ServicePriceLogs.Where(spl=>spl.ServicePriceId==id).AnyAsync();
            if (check)
            {
                return 0;
            }
            var spl = await _context.ServicePriceLists.Where(_spl => _spl.ServicePriceId==id).SingleOrDefaultAsync();
            if(spl.Status == 1)
            {
                return -1;
            }
            _context.ServicePriceLists.Remove(spl);
            await _context.SaveChangesAsync(); 
            return 1;
        }
    }
}
