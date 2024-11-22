using VJN.Models;
using VJN.ModelsDTO.ServicePriceListDTOs;

namespace VJN.Services
{
    public interface IServicePriceListService
    {
        public Task<IEnumerable<ServicePriceListDTO>> GetAllServicePriceList();
        public  Task<ServicePriceListDTO> GetServicePriceById(int id);
    }
}
