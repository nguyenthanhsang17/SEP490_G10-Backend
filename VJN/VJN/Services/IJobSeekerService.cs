using VJN.Models;
using VJN.ModelsDTO.FavoriteListDTOs;
using VJN.ModelsDTO.JobSeekerDTOs;
using VJN.Paging;

namespace VJN.Services
{
    public interface IJobSeekerService
    {
        public Task<bool> AddFavorite(FavoriteListCreateDTO model, int userid);
        public Task<bool> DeleteFavorite(int JobseekerID, int userid);
        public Task<PagedResult<JobSeekerDTO>> GetAllFavoriteList(FavoriteListSearch s, int userid);

        public Task<JobSeekerDetailDTO> GetJobSeekerByIserID(int userID);
    }
}
