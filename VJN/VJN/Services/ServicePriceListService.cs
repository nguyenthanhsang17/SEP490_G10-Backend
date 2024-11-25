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

        public async Task<ServicePriceList> CreateServicePriceList(ServicePriceList newServicePriceList)
        {
            var createdServicePriceList = await _servicePriceListRepository.CreateServicePriceList(newServicePriceList);
            return createdServicePriceList;
        }

        public async Task<bool> ChangeStatusPriceList(int id, int newStatus) 
        { 
            return await _servicePriceListRepository.ChangeStatusPriceList(id, newStatus);
        }

        public async Task<IEnumerable<ServicePriceListDTO>> GetAllServicePriceListWithStatus1()
        {
            var spl = await _servicePriceListRepository.GetAllServicePriceListWithStatus1();
            var pricesDTO = _mapper.Map<IEnumerable<ServicePriceListDTO>>(spl);
            return pricesDTO;
        }
    }
}
