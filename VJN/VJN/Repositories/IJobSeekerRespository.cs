using VJN.Models;
using VJN.ModelsDTO.CvDTOs;
using VJN.ModelsDTO.FavoriteListDTOs;

namespace VJN.Repositories
{
    public interface IJobSeekerRespository
    {
        public Task<bool> AddFavorite(FavoriteList model);

        public Task<bool> DeleteFavorite(int JobseekerID, int userid);

        public Task<IEnumerable<int>> GetAllFavoriteId(FavoriteListSearch model, int userid);
        public Task<IEnumerable<User>> GetAllFavorite(IEnumerable<int> ids);

        public Task<User> GetJobSeekerByIserID(int userID);

        public Task<IEnumerable<Cv>> GetCVByUserId(int userID);

        public Task<IEnumerable<int>> GetAllJobSeeker();
    }
}
