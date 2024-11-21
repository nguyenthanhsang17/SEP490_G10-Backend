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
    }
}
