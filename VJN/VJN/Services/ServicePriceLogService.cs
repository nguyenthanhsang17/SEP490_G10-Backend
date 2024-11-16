
using VJN.ModelsDTO.ServicePriceLogDTOs;
using VJN.Repositories;

namespace VJN.Services
{
    public class ServicePriceLogService : IServicePriceLogService
    {
        private readonly IServicePriceLogRepository _servicePriceLogRepository;

        public ServicePriceLogService(IServicePriceLogRepository servicePriceLogRepository)
        {
            _servicePriceLogRepository = servicePriceLogRepository;
        }

        public async Task<bool> Addition(int userid, bool check, int time)
        {
            var c  = await _servicePriceLogRepository.Addition(userid, check, time);
            return c;
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
