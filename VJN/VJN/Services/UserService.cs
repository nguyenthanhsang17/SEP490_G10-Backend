using AutoMapper;
using Microsoft.EntityFrameworkCore.Internal;
using VJN.Authenticate;
using VJN.Models;
using VJN.ModelsDTO.UserDTOs;
using VJN.Repositories;
using static System.Net.WebRequestMethods;

namespace VJN.Services
{
    public class UserService : IUserService
    {
        public readonly IUserRepository _userRepository;
        public readonly IRegisterEmployerRepository _registerEmployer;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IMapper mapper, IRegisterEmployerRepository registerEmployer) {
            _userRepository = userRepository;
            _mapper = mapper;
            _registerEmployer = registerEmployer;
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

        public async Task<bool> CheckOtpExits(string otp)
        {
            return await _userRepository.CheckOtpExits(otp);
        }

        public async Task<int> CreateUser(UserCreateDTO userdto, string otp)
        {
            if(userdto == null)
            {
                return 0;
            }
            if (!userdto.Password.Equals(userdto.ConfirmPassword))
            {
                return 1;
            }
            var check = await _userRepository.CheckEmailExits(userdto.Email);
            if (check)
            {
                return 2;
            }
            else
            {
                var user = _mapper.Map<User>(userdto);
                user.VerifyCode = otp;
                user.SendCodeTime = DateTime.Now;
                var i = await _userRepository.CreateUser(user);
                return 3;
            }
        }

        public async Task<UserDTO> findById(int id)
        {
            var user = await _userRepository.findById(id);
            var userdto = _mapper.Map<UserDTO>(user);
            var register = await _registerEmployer.GetRegisterEmployerByUserID(id);
            if (register != null)
            {
                userdto.RegisterEmployerStatus = register.Status;
                userdto.Reason = register.Reason;
            }
            return userdto;
        }

        public async Task<UserDTOdetail> GetUserDetail(int id)
        {
            var user = await _userRepository.findById(id);
            var userdtodetail = _mapper.Map<UserDTOdetail>(user);
            return userdtodetail;
        }

        public async Task<IEnumerable<UserDTO>> getAllUser()
        {
            var users = await _userRepository.getAllUser();
            var userdto = _mapper.Map<IEnumerable<UserDTO>>(users);
            return userdto;
        }

        public async Task<UserDTO> GetUserByEmail(string Email)
        {
            var user = await _userRepository.GetUserByEmail(Email);
            var userdto = _mapper.Map<UserDTO>(user);
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

        public async Task<bool> UpdatePassword(int userid, string password)
        {
            var check = await _userRepository.UpdatePassword(userid, password);
            return check;
        }

        public async Task<bool> UpdateProfile(int v, UserUpdateDTO model, int avatarID)
        {
            var user = _mapper.Map<User>(model);
            if(avatarID != 0)
            {
                user.Avatar = avatarID;
            }
            var check = await _userRepository.UpdateProfile(v, user);
            return check;
        }

        public async Task<int> UpdateStatusByEmail(string email, int status)
        {
            var id = await _userRepository.GetUserIdEmailExits(email);
            if (id == 0)
            {
                return 0;
            }
            var resuklt = await _userRepository.UpdateStatus(id, status);
            return resuklt == 1 ? 1 : 0;
        }

        public async Task<int> UpdateStatusByUid(int uid, int status)
        {
            var i = await _userRepository.UpdateStatus(uid, status);
            return i;
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
                await _userRepository.RemoveUserVerifycode(user);
                return true;
            }
        }

        public async Task<bool> VerifycodeRegister(string Otp)
        {
            var u = await _userRepository.GetUserByOtp(Otp);
            if (u == null)
            {
                return false;
            }
            else
            {
                int i = await _userRepository.UpdateStatus(u.UserId, 1);
                if (i == 1)
                {
                    await _userRepository.RemoveUserVerifycode(u);
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
