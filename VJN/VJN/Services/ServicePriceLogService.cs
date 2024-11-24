
using AutoMapper;
using VJN.Models;
using VJN.ModelsDTO.ServiceDTOs;
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

        public async Task<IEnumerable<PaymentHistory>> GetAllPaymentHistory()
        {
            var spl = await _servicePriceLogRepository.GetAllPaymentHistory();
            if (spl == null || spl.Count() == 0 || !spl.Any())
            {
                return null;
            }
            var pa = _mapper.Map<IEnumerable<PaymentHistory>>(spl);
            return pa;
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

        public async Task<bool> CreateServicePriceLog(int serviceid, int u)
        {
            var model = new ServicePriceLog
            {
                UserId = u,
                ServicePriceId = serviceid,
                RegisterDate = DateTime.Now,
            };

            var i = await _servicePriceLogRepository.CreateServicePriceLog(model);

            var i1 = await _servicePriceLogRepository.AddService(u, serviceid);

            return i&&i1;
        }

        public async Task<bool> CheckIsViewAllJobSeeker(int userid)
        {
            var check = await _servicePriceLogRepository.CheckIsViewAllJobSeeker(userid);
            return check;
        }

        public async Task<ServiceDTO> GetAllServiceByUserId(int userid)
        {
            var service  = await _servicePriceLogRepository.GetAllServiceByUserId(userid);
            if(service == null)
            {
                return null;
            }
            else
            {
                var servicedto = _mapper.Map<ServiceDTO>(service);
                return servicedto;
            }
        }
    }
}
