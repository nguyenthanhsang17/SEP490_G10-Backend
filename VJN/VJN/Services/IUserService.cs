﻿using VJN.Models;
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
        public Task<int> CreateUser(UserCreateDTO user);
        public Task<int> UpdateStatusByUid(int uid, int status);
        public Task<int> UpdateStatusByEmail(string email, int status);
    }
}
