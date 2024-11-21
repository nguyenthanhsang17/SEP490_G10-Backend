using VJN.Models;

namespace VJN.Repositories
{
    public interface IServicePriceListRepository
    {
        public Task<IEnumerable<ServicePriceList>> GetAllServicePriceList();
    }
}
