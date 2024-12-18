﻿using VJN.ModelsDTO.UserDTOs;
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
        public Task<int> CreateUser(User user);
        public Task<int> UpdateStatus(int uid, int status);
        public Task<bool> CheckEmailExits(string Email);
        public Task<bool> UpdateProfile(int v, User model);
        public Task<bool> UpdatePassword(int userid, string password);
        public Task<bool> RemoveUserVerifycode(User user);
        public Task<bool> CheckOtpExits(string otp);
        public Task<User> GetUserByOtp(string otp);

        public Task InsertOTP(int userid, string otp);
        public Task<IEnumerable<User>> GetAllUserWithoutAdmin();
        public Task<int> LoginWithGG(User model);
        public Task<int> CreateUserLoginWithGG(User user);

        public Task<string> GetPassword(int uid);
    }
}
