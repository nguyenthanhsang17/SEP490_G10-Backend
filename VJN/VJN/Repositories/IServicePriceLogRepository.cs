namespace VJN.Repositories
{
    public interface IServicePriceLogRepository
    {
        public Task<bool> subtraction(int userid, bool check, int time);
        public Task<bool> Addition(int userid, bool check, int time);
        public Task<int> GetNumberPosts(int userid);
        public Task<int> NumberPostsUrgentRecruitment(int userid);
        public Task<bool> CheckIsViewAllJobSeeker(int userid);
    }
}
