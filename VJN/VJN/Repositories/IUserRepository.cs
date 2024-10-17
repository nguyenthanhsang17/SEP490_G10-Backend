using VJN.ModelsDTO.UserDTOs;
using VJN.Models;

namespace VJN.Repositories
{
    public interface IUserRepository
    {
        public Task<IEnumerable<User>> getAllUser();

        public Task<User> findById(int id);

        public Task<User> Login(string Username, string Password);
    }
}
