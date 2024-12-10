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
    public class LoginWithGoogleClassTest
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IRegisterEmployerRepository> _registerEmployerMock;
        private readonly Mock<IMediaItemRepository> _mediaItemRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly UserService _userService;

        public LoginWithGoogleClassTest()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _registerEmployerMock = new Mock<IRegisterEmployerRepository>();
            _mediaItemRepositoryMock = new Mock<IMediaItemRepository>();
            _mapperMock = new Mock<IMapper>();
            _userService = new UserService(
                _userRepositoryMock.Object,
                _mapperMock.Object,
                _registerEmployerMock.Object,
                _mediaItemRepositoryMock.Object);
        }

        [Fact]
        public async Task LoginWithGG_ShouldReturnNull_WhenModelIsNull()
        {
            // Arrange
            UserLoginWithGG model = null;

            // Act
            var result = await _userService.LoginWithGG(model);

            // Assert
            Xunit.Assert.Null(result);
            _userRepositoryMock.Verify(repo => repo.CheckEmailExits(It.IsAny<string>()), Times.Never);
            _userRepositoryMock.Verify(repo => repo.GetUserByEmail(It.IsAny<string>()), Times.Never);
            _mediaItemRepositoryMock.Verify(repo => repo.CreateMediaItem(It.IsAny<MediaItem>()), Times.Never);
        }

        [Fact]
        public async Task LoginWithGG_ShouldReturnUserDTO_WhenEmailExists()
        {
            // Arrange
            var model = new UserLoginWithGG
            {
                Email = "existinguser@example.com",
                FullName = "Existing User",
                Avatar = "avatar_url"
            };

            var user = new User { Email = model.Email, FullName = model.FullName };
            var userDTO = new UserDTO { Email = model.Email, FullName = model.FullName };

            _userRepositoryMock.Setup(repo => repo.CheckEmailExits(model.Email)).ReturnsAsync(true);
            _userRepositoryMock.Setup(repo => repo.GetUserByEmail(model.Email)).ReturnsAsync(user);
            _mapperMock.Setup(mapper => mapper.Map<UserDTO>(user)).Returns(userDTO);

            // Act
            var result = await _userService.LoginWithGG(model);

            // Assert
            Xunit.Assert.NotNull(result);
            Xunit.Assert.Equal(userDTO.Email, result.Email);
            Xunit.Assert.Equal(userDTO.FullName, result.FullName);

            _userRepositoryMock.Verify(repo => repo.CheckEmailExits(model.Email), Times.Once);
            _userRepositoryMock.Verify(repo => repo.GetUserByEmail(model.Email), Times.Once);
            _mapperMock.Verify(mapper => mapper.Map<UserDTO>(user), Times.Once);
        }

        [Fact]
        public async Task LoginWithGG_ShouldCreateNewUser_WhenEmailDoesNotExist()
        {
            // Arrange
            var model = new UserLoginWithGG
            {
                Email = "newuser@example.com",
                FullName = "New User",
                Avatar = "avatar_url"
            };

            var media = new MediaItem { Url = model.Avatar, Status = true };
            var createdUser = new User { Email = model.Email, FullName = model.FullName };
            var userDTO = new UserDTO { Email = model.Email, FullName = model.FullName };

            _userRepositoryMock.Setup(repo => repo.CheckEmailExits(model.Email)).ReturnsAsync(false);
            _mediaItemRepositoryMock.Setup(repo => repo.CreateMediaItem(It.IsAny<MediaItem>())).ReturnsAsync(1); // Media ID = 1
            _userRepositoryMock.Setup(repo => repo.CreateUserLoginWithGG(It.IsAny<User>())).ReturnsAsync(1); // User ID = 1
            _userRepositoryMock.Setup(repo => repo.findById(1)).ReturnsAsync(createdUser);
            _mapperMock.Setup(mapper => mapper.Map<UserDTO>(createdUser)).Returns(userDTO);

            // Act
            var result = await _userService.LoginWithGG(model);

            // Assert
            Xunit.Assert.NotNull(result);
            Xunit.Assert.Equal(userDTO.Email, result.Email);
            Xunit.Assert.Equal(userDTO.FullName, result.FullName);

            _userRepositoryMock.Verify(repo => repo.CheckEmailExits(model.Email), Times.Once);
            _mediaItemRepositoryMock.Verify(repo => repo.CreateMediaItem(It.Is<MediaItem>(m => m.Url == model.Avatar)), Times.Once);
            _userRepositoryMock.Verify(repo => repo.CreateUserLoginWithGG(It.Is<User>(u => u.Email == model.Email)), Times.Once);
            _userRepositoryMock.Verify(repo => repo.findById(1), Times.Once);
            _mapperMock.Verify(mapper => mapper.Map<UserDTO>(createdUser), Times.Once);
        }
    }
}
