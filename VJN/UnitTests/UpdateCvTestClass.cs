using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using VJN.Models;
using VJN.ModelsDTO.CvDTOs;
using VJN.Repositories;
using VJN.Services;
using Xunit;

namespace UnitTests
{
    public class UpdateCvTestClass
    {
        private readonly Mock<ICvRepository> _cvRepositoryMock;
        private readonly CvService _cvService;

        public UpdateCvTestClass()
        {
            _cvRepositoryMock = new Mock<ICvRepository>();
            _cvService = new CvService(_cvRepositoryMock.Object);
        }

        [Fact]
        public async Task UpdateCv_ShouldCreateNewCv_WhenCvIdIsNegative()
        {
            // Arrange
            var cvDto = new CvDTODetail
            {
                CvId = -1,
                NameCv = "New CV",
                UserId = 1,
                ItemOfCvs = new List<VJN.ModelsDTO.ItemOfCvDTOs.ItemOfcvDTOforView>
                {
                    new VJN.ModelsDTO.ItemOfCvDTOs.ItemOfcvDTOforView
                    {
                        ItemName = "Skill",
                        ItemDescription = "C# Development"
                    }
                }
            };

            _cvRepositoryMock
                .Setup(repo => repo.CreateCv(It.IsAny<Cv>()))
                .ReturnsAsync(1); // ID của CV mới là 1

            // Act
            var result = await _cvService.UpdateCv(cvDto);

            // Assert
            Xunit.Assert.Equal(1, result); // Kỳ vọng ID trả về là 1
            _cvRepositoryMock.Verify(repo => repo.CreateCv(It.Is<Cv>(
                c => c.NameCv == "New CV" && c.UserId == 1 && c.ItemOfCvs.Count == 1)), Times.Once);
        }

        [Fact]
        public async Task UpdateCv_ShouldUpdateExistingCv_WhenCvIdIsPositive()
        {
            // Arrange
            var cvDto = new CvDTODetail
            {
                CvId = 2,
                NameCv = "Updated CV",
                UserId = 1,
                ItemOfCvs = new List<VJN.ModelsDTO.ItemOfCvDTOs.ItemOfcvDTOforView>
                {
                    new VJN.ModelsDTO.ItemOfCvDTOs.ItemOfcvDTOforView
                    {
                        ItemName = "Updated Skill",
                        ItemDescription = "Updated Description"
                    }
                }
            };

            _cvRepositoryMock
                .Setup(repo => repo.UpdateCv(It.IsAny<Cv>()))
                .ReturnsAsync(2); // ID của CV sau khi cập nhật là 2

            // Act
            var result = await _cvService.UpdateCv(cvDto);

            // Assert
            Xunit.Assert.Equal(2, result); // Kỳ vọng ID trả về là 2
            _cvRepositoryMock.Verify(repo => repo.UpdateCv(It.Is<Cv>(
                c => c.CvId == 2 && c.NameCv == "Updated CV" && c.UserId == 1)), Times.Once);
        }

        [Fact]
        public async Task UpdateCv_ShouldReturnZero_WhenCvDtoIsNull()
        {
            // Arrange
            CvDTODetail cvDto = null;

            // Act
            var result = await _cvService.UpdateCv(cvDto);

            // Assert
            Xunit.Assert.Equal(0, result); // Kỳ vọng ID trả về là 0
            _cvRepositoryMock.Verify(repo => repo.CreateCv(It.IsAny<Cv>()), Times.Never);
            _cvRepositoryMock.Verify(repo => repo.UpdateCv(It.IsAny<Cv>()), Times.Never);
        }

        [Fact]
        public async Task UpdateCv_ShouldHandleEmptyItems()
        {
            // Arrange
            var cvDto = new CvDTODetail
            {
                CvId = -1,
                NameCv = "CV Without Items",
                UserId = 1,
                ItemOfCvs = new List<VJN.ModelsDTO.ItemOfCvDTOs.ItemOfcvDTOforView>() // Không có items
            };

            _cvRepositoryMock
                .Setup(repo => repo.CreateCv(It.IsAny<Cv>()))
                .ReturnsAsync(3); // ID của CV mới là 3

            // Act
            var result = await _cvService.UpdateCv(cvDto);

            // Assert
            Xunit. Assert.Equal(3, result); // Kỳ vọng ID trả về là 3
            _cvRepositoryMock.Verify(repo => repo.CreateCv(It.Is<Cv>(
                c => c.NameCv == "CV Without Items" && c.UserId == 1 && c.ItemOfCvs.Count == 0)), Times.Once);
        }
    }
}
