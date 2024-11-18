using VJN.Models;
using VJN.ModelsDTO.EmployerDTOs;

namespace VJN.Services
{
    public interface IEmployerService
    {
        public Task<EmployerDTO> GetEmployerByUserId(int id, int? userid, decimal? Latitude, decimal? Longitude);
    }
}
