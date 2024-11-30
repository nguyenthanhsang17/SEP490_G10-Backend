using AutoMapper;
using Moq;
using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VJN.Models;
using VJN.ModelsDTO.UserDTOs;
using VJN.Repositories;
using VJN.Services;

namespace UnitTests
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDTO>();
        }
    }
    public class UserServiceTest
    {
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IRegisterEmployerRepository> _mockRegisterEmployer;
        private readonly UserService _userService;

        public UserServiceTest()
        {

            _mockUserRepository = new Mock<IUserRepository>();
            _mockMapper = new Mock<IMapper>();
            _mockRegisterEmployer = new Mock<IRegisterEmployerRepository>();

            _userService = new UserService(
                _mockUserRepository.Object,
                _mockMapper.Object,
                _mockRegisterEmployer.Object);
        }



        [Fact]
        public async Task Login_ValidCredentials_ReturnsUserDTO()
        {
            // Arrange
            var testUser = new User
            {
                UserId = 1,
                Email = "test@example.com",
                Password = "password123",
                Role = new Role { RoleId = 1, RoleName = "Admin" }
            };

            var testUserDTO = new UserDTO
            {
                UserId = 1,
                Email = "test@example.com",
                RoleName = "Admin"
            };

            _mockUserRepository
                .Setup(repo => repo.Login(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(testUser);

            _mockMapper
                .Setup(mapper => mapper.Map<UserDTO>(It.IsAny<User>()))
                .Returns(testUserDTO);

            // Act
            var result = await _userService.Login("test@example.com", "password123");

            // Assert
            Xunit.Assert.NotNull(result);
            Xunit.Assert.Equal(testUserDTO.Email, result.Email);
            Xunit.Assert.Equal(testUserDTO.RoleName, result.RoleName);
        }

        [Fact]
        public async Task GetAllUser_ReturnsListOfUserDTOs()
        {
            // Arrange
            var testUsers = new List<User>
            {
                new User
                {
                    UserId = 1,
                    Email = "user1@example.com",
                    Role = new Role { RoleId = 1, RoleName = "Admin" },
                    Avatar = 100
                },
                new User
                {
                    UserId = 2,
                    Email = "user2@example.com",
                    Role = new Role { RoleId = 2, RoleName = "User" },
                    Avatar = 101
                }
            };

            var testUserDTOs = new List<UserDTO>
            {
                new UserDTO
                {
                    UserId = 1,
                    Email = "user1@example.com",
                    RoleName = "Admin",
                    Avatar = 100
                },
                new UserDTO
                {
                    UserId = 2,
                    Email = "user2@example.com",
                    RoleName = "User",
                    Avatar = 101
                }
            };

            _mockUserRepository
                .Setup(repo => repo.getAllUser())
                .ReturnsAsync(testUsers);

            _mockMapper
                .Setup(mapper => mapper.Map<IEnumerable<UserDTO>>(testUsers))
                .Returns(testUserDTOs);

            // Act
            var result = await _userService.getAllUser();

            // Assert
            Xunit.Assert.NotNull(result);
            Xunit.Assert.Equal(testUserDTOs.Count, result.Count());
            Xunit.Assert.Equal(testUserDTOs.First().Email, result.First().Email);
            Xunit.Assert.Equal(testUserDTOs.Last().RoleName, result.Last().RoleName);
        }

        [Fact]
        public async Task UpdateOtpUser_ValidEmailAndOtp_ReturnsSuccess()
        {
            var email = "user@example.com";
            var otp = "123456";
            var userId = 1;

            _mockUserRepository
                .Setup(repo => repo.GetUserIdEmailExits(email))
                .ReturnsAsync(userId);

            _mockUserRepository
                .Setup(repo => repo.UpdateOtpUser(userId, otp))
                .ReturnsAsync(1); 

            var result = await _userService.UpdateOtpUser(email, otp);

            Xunit.Assert.Equal(1, result); 
        }

        [Fact]
        public async Task UpdateOtpUser_InvalidEmail_ReturnsFailure()
        {
            // Arrange
            var email = "invalid@example.com";
            var otp = "123456";

            _mockUserRepository
                .Setup(repo => repo.GetUserIdEmailExits(email))
                .ReturnsAsync(0); 

            var result = await _userService.UpdateOtpUser(email, otp);


            Xunit.Assert.Equal(0, result); 
        }

        [Fact]
        public async Task UpdateOtpUser_ValidEmailButUpdateFails_ReturnsFailure()
        {
            // Arrange
            var email = "user@example.com";
            var otp = "123456";
            var userId = 1;

            _mockUserRepository
                .Setup(repo => repo.GetUserIdEmailExits(email))
                .ReturnsAsync(userId);

            _mockUserRepository
                .Setup(repo => repo.UpdateOtpUser(userId, otp))
                .ReturnsAsync(0); 

            var result = await _userService.UpdateOtpUser(email, otp);

            Xunit.Assert.Equal(0, result); 
        }


    }
}
