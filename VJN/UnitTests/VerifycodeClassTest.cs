using AutoMapper;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VJN.Models;
using VJN.Repositories;
using VJN.Services;
using Xunit;
using static System.Net.WebRequestMethods;

namespace UnitTests
{
    public class VerifycodeClassTest
    {

        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IRegisterEmployerRepository> _mockRegisterEmployer;
        private readonly UserService _userService;

        public VerifycodeClassTest()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _mockMapper = new Mock<IMapper>();
            _mockRegisterEmployer = new Mock<IRegisterEmployerRepository>();
            _userService = new UserService(
                _mockUserRepository.Object,
                _mockMapper.Object,
                _mockRegisterEmployer.Object);
        }
        #region Verifycode Tests

        [Fact]
        public async Task Verifycode_ValidOtp_ReturnsTrue()
        {
            // Arrange
            var email = "test@example.com";
            var otp = "123456";
            var user = new User
            {
                UserId = 1,
                Email = email,
                VerifyCode = otp,
                SendCodeTime = DateTime.Now.AddMinutes(-3) // Within 5 minutes
            };

            _mockUserRepository.Setup(repo => repo.GetUserByEmail(email)).ReturnsAsync(user);
            _mockUserRepository.Setup(repo => repo.RemoveUserVerifycode(user)).ReturnsAsync(true); // Hoặc false nếu cần

            // Act
            var result = await _userService.Verifycode(email, otp);
            // Assert
            Xunit.Assert.True(result);
        }

        [Fact]
        public async Task Verifycode_ValidOtp_ReturnsTrue1()
        {
            // Arrange
            var email = "test@example.com";
            var otp = "123456";
            var user = new User
            {
                UserId = 1,
                Email = email,
                VerifyCode = otp,
                SendCodeTime = DateTime.Now.AddMinutes(-4.8) // Within 5 minutes
            };

            _mockUserRepository.Setup(repo => repo.GetUserByEmail(email)).ReturnsAsync(user);
            _mockUserRepository.Setup(repo => repo.RemoveUserVerifycode(user)).ReturnsAsync(true); // Hoặc false nếu cần

            // Act
            var result = await _userService.Verifycode(email, otp);
            // Assert
            Xunit.Assert.True(result);
        }

        [Fact]
        public async Task Verifycode_InvalidOtp_ReturnsFalse()
        {
            // Arrange
            var email = "test@example.com";
            var otp = "1234567";
            var user = new User
            {
                UserId = 1,
                Email = email,
                VerifyCode = "123456",
                SendCodeTime = DateTime.Now.AddMinutes(-4)
            };

            _mockUserRepository.Setup(repo => repo.GetUserByEmail(email)).ReturnsAsync(user);

            // Act
            var result = await _userService.Verifycode(email, otp);

            // Assert
            Xunit.Assert.False(result); // OTP mismatch
        }

        [Fact]
        public async Task Verifycode_ExpiredOtp_ReturnsFalse()
        {
            // Arrange
            var email = "test@example.com";
            var otp = "123456";
            var user = new User
            {
                UserId = 1,
                Email = email,
                VerifyCode = otp,
                SendCodeTime = DateTime.Now.AddMinutes(-6) // Beyond 5 minutes
            };

            _mockUserRepository.Setup(repo => repo.GetUserByEmail(email)).ReturnsAsync(user);

            // Act
            var result = await _userService.Verifycode(email, otp);

            // Assert
            Xunit.Assert.False(result); // OTP expired
        }

        [Fact]
        public async Task Verifycode_UserNotFound_ReturnsFalse()
        {
            var email = "test1@example.com";
            var user = new User
            {
                UserId = 1,
                Email = email,
                VerifyCode = "anyOtp",
                SendCodeTime = DateTime.Now.AddMinutes(-6) // Beyond 5 minutes
            };
            _mockUserRepository.Setup(repo => repo.GetUserByEmail(email)).ReturnsAsync((User)null);

            var result = await _userService.Verifycode(email, "anyOtp");

            Xunit.Assert.False(result);
        }
        #endregion
    }
}
