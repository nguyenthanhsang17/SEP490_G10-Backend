using AutoMapper;
using VJN.Models;
using VJN.ModelsDTO.ServicePriceListDTOs;
using VJN.Repositories;

namespace VJN.Services
{
    public class ServicePriceListService : IServicePriceListService
    {
        private readonly IServicePriceListRepository _servicePriceListRepository;
        private readonly IMapper _mapper;

        public ServicePriceListService(IServicePriceListRepository servicePriceListRepository, IMapper mapper)
        {
            _servicePriceListRepository = servicePriceListRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ServicePriceListDTO>> GetAllServicePriceList()
        {
            var prices = await _servicePriceListRepository.GetAllServicePriceList();
            var pricesDTO = _mapper.Map<IEnumerable<ServicePriceListDTO>>(prices);
            return pricesDTO;
        }

        public async Task<ServicePriceListDTO> GetServicePriceById(int id)
        {
            var prices = await _servicePriceListRepository.GetAllServicePriceList();
            var pricesDTO = _mapper.Map<IEnumerable<ServicePriceListDTO>>(prices);
            var result = pricesDTO.FirstOrDefault(r=>r.ServicePriceId == id);
            return result;
        }
    }
}
