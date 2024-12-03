using AutoMapper;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VJN.Models;
using VJN.ModelsDTO.UserDTOs;
using VJN.Repositories;
using VJN.Services;
using Xunit;

namespace UnitTests
{
    public class CreateUserClassTest
    {
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IRegisterEmployerRepository> _mockRegisterEmployer;
        private readonly UserService _userService;

        public CreateUserClassTest()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _mockMapper = new Mock<IMapper>();
            _mockRegisterEmployer = new Mock<IRegisterEmployerRepository>();
            _userService = new UserService(
                _mockUserRepository.Object,
                _mockMapper.Object,
                _mockRegisterEmployer.Object);
        }

        #region CreateUser Tests

        [Fact]
        public async Task CreateUser_NullUserDTO_ReturnsZero()
        {
            // Arrange
            UserCreateDTO userdto = null;
            string otp = "123456";

            // Act
            var result = await _userService.CreateUser(userdto, otp);

            // Assert
            Xunit.Assert.Equal(0, result);
        }

        [Fact]
        public async Task CreateUser_PasswordMismatch_ReturnsOne()
        {
            // Arrange
            var userdto = new UserCreateDTO
            {
                Email = "test@example.com",
                Password = "password123",
                ConfirmPassword = "password456" // Mật khẩu không khớp
            };
            string otp = "123456";

            // Act
            var result = await _userService.CreateUser(userdto, otp);

            // Assert
            Xunit.Assert.Equal(1, result);
        }

        [Fact]
        public async Task CreateUser_EmailExists_ReturnsTwo()
        {
            // Arrange
            var userdto = new UserCreateDTO
            {
                Email = "test@example.com",
                Password = "password123",
                ConfirmPassword = "password123"
            };
            string otp = "123456";

            _mockUserRepository.Setup(repo => repo.CheckEmailExits(userdto.Email)).ReturnsAsync(true);

            // Act
            var result = await _userService.CreateUser(userdto, otp);

            // Assert
            Xunit.Assert.Equal(2, result);
        }

        [Fact]
        public async Task CreateUser_Success_ReturnsThree()
        {
            // Arrange
            var userdto = new UserCreateDTO
            {
                Email = "test@example.com",
                Password = "password123",
                ConfirmPassword = "password123"
            };
            string otp = "123456";

            _mockUserRepository.Setup(repo => repo.CheckEmailExits(userdto.Email)).ReturnsAsync(false);
            _mockUserRepository.Setup(repo => repo.CreateUser(It.IsAny<User>())).ReturnsAsync(1);

            var user = new User
            {
                Email = userdto.Email,
                VerifyCode = otp
            };

            _mockMapper.Setup(mapper => mapper.Map<User>(userdto)).Returns(user);

            // Act
            var result = await _userService.CreateUser(userdto, otp);

            // Assert
            Xunit.Assert.Equal(3, result);
        }

        #endregion
    }
}
