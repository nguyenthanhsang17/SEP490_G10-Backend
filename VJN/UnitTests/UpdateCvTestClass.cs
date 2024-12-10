using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
                .ReturnsAsync(true);

            // Act
            var result = await _cvService.UpdateCv(cvDto);

            // Assert
            Xunit.Assert.True(result);
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
                .ReturnsAsync(true);

            // Act
            var result = await _cvService.UpdateCv(cvDto);

            // Assert
            Xunit.Assert.True(result);
            _cvRepositoryMock.Verify(repo => repo.UpdateCv(It.Is<Cv>(
                c => c.CvId == 2 && c.NameCv == "Updated CV" && c.UserId == 1)), Times.Once);
        }

        [Fact]
        public async Task UpdateCv_ShouldReturnFalse_WhenRepositoryFails()
        {
            // Arrange
            var cvDto = new CvDTODetail
            {
                CvId = 2,
                NameCv = "Failing CV",
                UserId = 1,
                ItemOfCvs = new List<VJN.ModelsDTO.ItemOfCvDTOs.ItemOfcvDTOforView>()
            };

            _cvRepositoryMock
                .Setup(repo => repo.UpdateCv(It.IsAny<Cv>()))
                .ReturnsAsync(false);

            // Act
            var result = await _cvService.UpdateCv(cvDto);

            // Assert
            Xunit.Assert.False(result);
            _cvRepositoryMock.Verify(repo => repo.UpdateCv(It.IsAny<Cv>()), Times.Once);
        }

        [Fact]
        public async Task UpdateCv_ShouldCreateCv_WhenNoItemsProvided()
        {
            // Arrange
            var cvDto = new CvDTODetail
            {
                CvId = -1,
                NameCv = "New CV Without Items",
                UserId = 1,
                ItemOfCvs = new List<VJN.ModelsDTO.ItemOfCvDTOs.ItemOfcvDTOforView>()
            };

            _cvRepositoryMock
                .Setup(repo => repo.CreateCv(It.IsAny<Cv>()))
                .ReturnsAsync(true);

            // Act
            var result = await _cvService.UpdateCv(cvDto);

            // Assert
            Xunit.Assert.True(result);
            _cvRepositoryMock.Verify(repo => repo.CreateCv(It.Is<Cv>(
                c => c.NameCv == "New CV Without Items" && c.UserId == 1 && c.ItemOfCvs.Count == 0)), Times.Once);
        }

        [Fact]
        public async Task UpdateCv_ShouldUpdateCv_WithMultipleItems()
        {
            // Arrange
            var cvDto = new CvDTODetail
            {
                CvId = 10,
                NameCv = "Complex CV",
                UserId = 1,
                ItemOfCvs = new List<VJN.ModelsDTO.ItemOfCvDTOs.ItemOfcvDTOforView>
        {
            new VJN.ModelsDTO.ItemOfCvDTOs.ItemOfcvDTOforView { ItemName = "Skill 1", ItemDescription = "Description 1" },
            new VJN.ModelsDTO.ItemOfCvDTOs.ItemOfcvDTOforView { ItemName = "Skill 2", ItemDescription = "Description 2" }
        }
            };

            _cvRepositoryMock
                .Setup(repo => repo.UpdateCv(It.IsAny<Cv>()))
                .ReturnsAsync(true);

            // Act
            var result = await _cvService.UpdateCv(cvDto);

            // Assert
            Xunit.Assert.True(result);
            _cvRepositoryMock.Verify(repo => repo.UpdateCv(It.Is<Cv>(
                c => c.CvId == 10 && c.NameCv == "Complex CV" && c.ItemOfCvs.Count == 2)), Times.Once);
        }

        [Fact]
        public async Task UpdateCv_ShouldReturnFalse_WhenCvDtoIsNull()
        {
            // Arrange
            CvDTODetail cvDto = null;

            // Act
            var result = await _cvService.UpdateCv(cvDto);

            // Assert
            Xunit.Assert.False(result);
            _cvRepositoryMock.Verify(repo => repo.CreateCv(It.IsAny<Cv>()), Times.Never);
            _cvRepositoryMock.Verify(repo => repo.UpdateCv(It.IsAny<Cv>()), Times.Never);
        }

        

        

        [Fact]
        public async Task UpdateCv_ShouldHandleEmptyStrings()
        {
            // Arrange
            var cvDto = new CvDTODetail
            {
                CvId = 1,
                NameCv = string.Empty, // Empty NameCv
                UserId = 1,
                ItemOfCvs = new List<VJN.ModelsDTO.ItemOfCvDTOs.ItemOfcvDTOforView>
        {
            new VJN.ModelsDTO.ItemOfCvDTOs.ItemOfcvDTOforView
            {
                ItemName = string.Empty, // Empty ItemName
                ItemDescription = "Description"
            }
        }
            };

            _cvRepositoryMock
                .Setup(repo => repo.UpdateCv(It.IsAny<Cv>()))
                .ReturnsAsync(true);

            // Act
            var result = await _cvService.UpdateCv(cvDto);

            // Assert
            Xunit.Assert.True(result);
            _cvRepositoryMock.Verify(repo => repo.UpdateCv(It.Is<Cv>(
                c => c.CvId == 1 && c.NameCv == string.Empty && c.ItemOfCvs.FirstOrDefault().ItemName == string.Empty)), Times.Once);
        }

    }
}
