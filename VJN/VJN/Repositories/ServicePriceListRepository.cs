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

    }
}
