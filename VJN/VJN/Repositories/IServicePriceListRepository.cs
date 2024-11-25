using VJN.Models;

namespace VJN.Repositories
{
    public interface IServicePriceListRepository
    {
        public Task<IEnumerable<ServicePriceList>> GetAllServicePriceList();
        public Task<ServicePriceList> GetServicePriceList(int servicePriceListId);
        public  Task<ServicePriceList> CreateServicePriceList(ServicePriceList newServicePriceList);
        public Task<bool> ChangeStatusPriceList(int id, int newStatus);
        public Task<IEnumerable<ServicePriceList>> GetAllServicePriceListWithStatus1();
    }
}
