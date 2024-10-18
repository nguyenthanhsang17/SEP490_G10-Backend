using AutoMapper;
using VJN.Authenticate;
using VJN.ModelsDTO.UserDTOs;
using VJN.Repositories;

namespace VJN.Services
{
    public class UserService : IUserService
    {
        public readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IMapper mapper) {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<int> ChangePassword(string OldPassword, string NewPassword, string ConfirmPassword, int userid)
        {
            var user = await _userRepository.findById(userid);
            if(user == null)
            {
                return 0;
            }
            if(user.Password != OldPassword) {
                return -1;
            }
            if(NewPassword != ConfirmPassword)
            {
                return -2;
            }
            int result = await _userRepository.ChangePassword(NewPassword, userid);
            return result;
        }

        public async Task<bool> CheckEmailExits(string Email)
        {
            var id  = await _userRepository.GetUserIdEmailExits(Email);
            if (id == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public async Task<UserDTO> findById(int id)
        {
            var user = await _userRepository.findById(id);
            var userdto = _mapper.Map<UserDTO>(user);
            return userdto;
        }

        public async Task<IEnumerable<UserDTO>> getAllUser()
        {
            var users = await _userRepository.getAllUser();
            var userdto = _mapper.Map<IEnumerable<UserDTO>>(users);
            return userdto;
        }

        public async Task<UserDTO> Login(string Username, string Password)
        {
            var user = await _userRepository.Login(Username, Password);
            var UserDTO = _mapper.Map<UserDTO>(user);
            return UserDTO;
        }

        public async Task<int> UpdateOtpUser(string email, string otp)
        {
            var id = await _userRepository.GetUserIdEmailExits(email);
            if(id == 0)
            {
                return 0;
            }
            var resuklt = await _userRepository.UpdateOtpUser(id, otp);
            return resuklt==1?1:0;

        }

        public async Task<bool> Verifycode(string Email, string Otp)
        {
            var user = await _userRepository.GetUserByEmail(Email);
            var diffTime = DateTime.Now - user.SendCodeTime;
            if(diffTime.HasValue && Math.Abs(diffTime.Value.TotalMinutes) >= 5)
            {
                return false;
            }
            else if(!user.VerifyCode.Equals(Otp))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
