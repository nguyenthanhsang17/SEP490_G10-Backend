using VJN.ModelsDTO.UserDTOs;
using VJN.Models;

namespace VJN.Repositories
{
    public interface IUserRepository
    {
        public Task<IEnumerable<User>> getAllUser();

        public Task<User> findById(int id);

        public Task<User> Login(string Username, string Password);

        public Task<int> ChangePassword(string NewPassword, int userid);

        public Task<int> UpdateOtpUser(int userid, string otp);

        public Task<int> GetUserIdEmailExits(string Email);

        public Task<User> GetUserByEmail(string email);
    }
}
