﻿using VJN.Models;
using VJN.ModelsDTO.ServiceDTOs;
using VJN.ModelsDTO.ServicePriceLogDTOs;

namespace VJN.Repositories
{
    public interface IServicePriceLogRepository
    {
        public Task<bool> subtraction(int userid, bool check, int time);
        public Task<bool> Addition(int userid, bool check, int time);
        public Task<int> GetNumberPosts(int userid);
        public Task<int> NumberPostsUrgentRecruitment(int userid);
        public Task<bool> CheckIsViewAllJobSeeker(int userid);
        public Task<IEnumerable<ServicePriceLog>> GetPaymentHistory(int userid);
        public  Task<IEnumerable<ServicePriceLog>> GetAllPaymentHistory();
        public Task<bool> CreateServicePriceLog(ServicePriceLog model);
        public Task<bool> AddService(int userid, int ServicePriceID);
        public Task<Service> GetAllServiceByUserId(int userid);
    }
}
