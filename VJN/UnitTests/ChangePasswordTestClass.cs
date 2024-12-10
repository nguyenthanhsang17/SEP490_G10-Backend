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

namespace UnitTests
{
    public class ChangePasswordTestClass
    {
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IRegisterEmployerRepository> _mockRegisterEmployer;
        private readonly UserService _userService;

        public ChangePasswordTestClass()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _mockMapper = new Mock<IMapper>();
            _mockRegisterEmployer = new Mock<IRegisterEmployerRepository>();
            _userService = new UserService(
                _mockUserRepository.Object,
                _mockMapper.Object,
                _mockRegisterEmployer.Object);
        }

        #region ChangePassword Tests

        [Fact]
        public async Task ChangePassword_ValidData_ReturnsSuccess()
        {
            // Arrange
            var userId = 1;
            var oldPassword = "oldPass";
            var newPassword = "newPass";
            var confirmPassword = "newPass";
            var user = new User { UserId = userId, Password = oldPassword };

            _mockUserRepository.Setup(repo => repo.findById(userId)).ReturnsAsync(user);
            _mockUserRepository.Setup(repo => repo.ChangePassword(newPassword, userId)).ReturnsAsync(1);

            // Act
            var result = await _userService.ChangePassword(oldPassword, newPassword, confirmPassword, userId);

            // Assert
            Xunit.Assert.Equal(1, result); // Success
        }

        [Fact]
        public async Task ChangePassword_UserNotFound_ReturnsZero()
        {
            // Arrange
            var userId = 99;

            _mockUserRepository.Setup(repo => repo.findById(userId)).ReturnsAsync((User)null);

            // Act
            var result = await _userService.ChangePassword("any", "new", "new", userId);

            // Assert
            Xunit.Assert.Equal(0, result); // User not found
        }

        [Fact]
        public async Task ChangePassword_OldPasswordMismatch_ReturnsMinusOne()
        {
            // Arrange
            var userId = 1;
            var user = new User { UserId = userId, Password = "correctOldPass" };

            _mockUserRepository.Setup(repo => repo.findById(userId)).ReturnsAsync(user);

            // Act
            var result = await _userService.ChangePassword("wrongOldPass", "new", "new", userId);

            // Assert
            Xunit.Assert.Equal(-1, result); // Old password mismatch
        }

        [Fact]
        public async Task ChangePassword_NewPasswordMismatch_ReturnsMinusTwo()
        {
            // Arrange
            var userId = 1;
            var user = new User { UserId = userId, Password = "correctOldPass" };

            _mockUserRepository.Setup(repo => repo.findById(userId)).ReturnsAsync(user);

            // Act
            var result = await _userService.ChangePassword("correctOldPass", "newPass", "wrongConfirmPass", userId);

            // Assert
            Xunit.Assert.Equal(-2, result); // New password mismatch
        }

        #endregion
    }
}
