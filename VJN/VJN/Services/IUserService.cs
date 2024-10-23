using VJN.Models;
using VJN.ModelsDTO.UserDTOs;

namespace VJN.Services
{
    public interface IUserService
    {
        public Task<IEnumerable<UserDTO>> getAllUser();
        public Task<UserDTO> findById(int id);
        public Task<UserDTO> Login(string Username, string Password);
        public Task<int> ChangePassword(string OldPassword, string NewPassword, string ConfirmPassword, int userid);
        public Task<int> UpdateOtpUser(string email, string otp);
        public Task<bool> CheckEmailExits(string Email);
        public Task<bool> Verifycode(string Email, string Otp);
        public Task<bool> VerifycodeRegister(string Otp);
        public Task<int> CreateUser(UserCreateDTO userdto, string otp);
        public Task<int> UpdateStatusByUid(int uid, int status);
        public Task<int> UpdateStatusByEmail(string email, int status);
        public Task<bool> UpdateProfile(int v, UserUpdateDTO model);
        public Task<UserDTO> GetUserByEmail(string Email);
        public Task<bool> UpdatePassword(int userid, string password);
        public Task<bool> CheckOtpExits(string otp);
       public Task<UserDTOdetail> GetUserDetail(int id);
    }
}
