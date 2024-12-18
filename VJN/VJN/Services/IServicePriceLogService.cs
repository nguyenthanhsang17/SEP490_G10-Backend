﻿using VJN.Models;
using VJN.ModelsDTO.ServiceDTOs;
using VJN.ModelsDTO.ServicePriceLogDTOs;

namespace VJN.Services
{
    public interface IServicePriceLogService
    {
        public Task<bool> subtraction(int userid, bool check, int time);
        public Task<bool> Addition(int userid, bool check, int time);

        public Task<ServicePriceLogDTO> GetPriced(int userid);
        public Task<IEnumerable<PaymentHistory>> GetPaymentHistory(int userid);

        public  Task<IEnumerable<PaymentHistory>> GetAllPaymentHistory();

        public Task<bool> CreateServicePriceLog(int Serviceid, int uid);

        public Task<bool> CheckIsViewAllJobSeeker(int userid);

        public Task<ServiceDTO> GetAllServiceByUserId(int userid);
    }
}
