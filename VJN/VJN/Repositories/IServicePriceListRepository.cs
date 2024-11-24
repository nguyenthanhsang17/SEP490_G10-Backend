using VJN.Models;

namespace VJN.Repositories
{
    public interface IServicePriceListRepository
    {
        public Task<IEnumerable<ServicePriceList>> GetAllServicePriceList();
        public Task<ServicePriceList> GetServicePriceList(int servicePriceListId);
        public  Task<ServicePriceList> CreateServicePriceList(ServicePriceList newServicePriceList);
    }
}
