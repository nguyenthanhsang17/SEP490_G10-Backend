using VJN.Models;
using VJN.ModelsDTO.UserDTOs;

namespace VJN.Services
{
    public interface IUserService
    {
        public Task<IEnumerable<UserDTO>> getAllUser();
        public Task<UserDTO> findById(int id);
        public Task<UserDTO> Login(string Username, string Password);
    }
}
