
using AutoMapper;
using VJN.ModelsDTO.ServicePriceLogDTOs;
using VJN.Repositories;

namespace VJN.Services
{
    public class ServicePriceLogService : IServicePriceLogService
    {
        private readonly IServicePriceLogRepository _servicePriceLogRepository;
        private readonly IMapper _mapper;

        public ServicePriceLogService(IServicePriceLogRepository servicePriceLogRepository, IMapper mapper)
        {
            _servicePriceLogRepository = servicePriceLogRepository;
            _mapper = mapper;
        }

        public async Task<bool> Addition(int userid, bool check, int time)
        {
            var c  = await _servicePriceLogRepository.Addition(userid, check, time);
            return c;
        }

        public async Task<IEnumerable<PaymentHistory>> GetPaymentHistory(int userid)
        {
            var spl = await _servicePriceLogRepository.GetPaymentHistory(userid);
            if(spl == null|| spl.Count() ==0 || !spl.Any())
            {
                return null;
            }
            var ph = _mapper.Map<IEnumerable<PaymentHistory>>(spl);
            return ph;
        }

        public Task<ServicePriceLogDTO> GetPriced(int userid)
        {
            return null;
        }

        public async Task<bool> subtraction(int userid, bool check, int time)
        {
            var c = await _servicePriceLogRepository.subtraction(userid, check, time);
            return c;
        }
    }
}
