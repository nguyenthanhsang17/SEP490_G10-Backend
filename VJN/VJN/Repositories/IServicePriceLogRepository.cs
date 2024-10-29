namespace VJN.Repositories
{
    public interface IServicePriceLogRepository
    {
        public Task<bool> isexpired(int userid);

        public Task<bool> subtraction(int userid, bool check);
    }
}
