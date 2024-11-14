namespace VJN.Services
{
    public interface IServicePriceLogService
    {
        public Task<bool> subtraction(int userid, bool check, int time);
        public Task<bool> Addition(int userid, bool check, int time);
    }
}
