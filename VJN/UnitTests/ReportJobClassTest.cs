using AutoMapper;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VJN.Models;
using VJN.ModelsDTO.ReportDTO;
using VJN.Repositories;
using VJN.Services;
using Xunit;

namespace UnitTests
{
    public class ReportJobClassTest
    {
        private readonly Mock<IPostJobRepository> _mockPostJobRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IServicePriceLogRepository> _mockServicePriceLogRepository;
        private readonly Mock<IImagePostJobRepository> _mockImagePostJobRepository;
        private readonly Mock<ISlotRepository> _mockSlotRepository;
        private readonly Mock<IJobPostDateRepository> _mockJobPostDateRepository;
        private readonly Mock<VJNDBContext> _mockContext;
        private readonly Mock<IUserService> _mockUserService;
        private readonly PostJobService _postJobService;

        public ReportJobClassTest()
        {
            _mockPostJobRepository = new Mock<IPostJobRepository>();
            _mockMapper = new Mock<IMapper>();
            _mockServicePriceLogRepository = new Mock<IServicePriceLogRepository>();
            _mockImagePostJobRepository = new Mock<IImagePostJobRepository>();
            _mockSlotRepository = new Mock<ISlotRepository>();
            _mockJobPostDateRepository = new Mock<IJobPostDateRepository>();
            _mockContext = new Mock<VJNDBContext>();
            _mockUserService = new Mock<IUserService>();

            _postJobService = new PostJobService(
                _mockPostJobRepository.Object,
                _mockMapper.Object,
                _mockServicePriceLogRepository.Object,
                _mockImagePostJobRepository.Object,
                _mockSlotRepository.Object,
                _mockJobPostDateRepository.Object,
                _mockContext.Object,
                _mockUserService.Object
            );
        }

        #region ReportJob Tests

        [Fact]
        public async Task ReportJob_ValidReport_ReturnsReportId()
        {
            // Arrange
            int userId = 1;
            var reportCreateDto = new ReportCreateDTO
            {
                PostId = 100,
                Reason = "Test Report"
            };

            var reportModel = new Report
            {
                JobSeekerId = userId,
                PostId = reportCreateDto.PostId,
                Reason = reportCreateDto.Reason,
                CreateDate = DateTime.Now,
                Status = 1
            };

            _mockPostJobRepository
                .Setup(repo => repo.ReportJob(It.IsAny<Report>()))
                .ReturnsAsync(1);

            // Act
            var result = await _postJobService.ReportJob(reportCreateDto, userId);

            // Assert
            Xunit.Assert.Equal(1, result);
            _mockPostJobRepository.Verify(repo => repo.ReportJob(It.Is<Report>(
                r => r.JobSeekerId == userId &&
                r.PostId == reportCreateDto.PostId &&
                r.Reason == reportCreateDto.Reason
            )), Times.Once);
        }

        [Fact]
        public async Task ReportJob_RepositoryReturnsNegative_ReturnsNegativeValue()
        {
            // Arrange
            int userId = 1;
            var reportCreateDto = new ReportCreateDTO
            {
                PostId = 100,
                Reason = "Test Report"
            };

            _mockPostJobRepository
                .Setup(repo => repo.ReportJob(It.IsAny<Report>()))
                .ReturnsAsync(-1);

            // Act
            var result = await _postJobService.ReportJob(reportCreateDto, userId);

            // Assert
            Xunit.Assert.Equal(-1, result);
            _mockPostJobRepository.Verify(repo => repo.ReportJob(It.IsAny<Report>()), Times.Once);
        }

        

        [Fact]
        public async Task ReportJob_InvalidUserId_ReturnsNegativeValue()
        {
            // Arrange
            int invalidUserId = 0;
            var reportCreateDto = new ReportCreateDTO
            {
                PostId = 100,
                Reason = "Test Report"
            };

            _mockPostJobRepository
                .Setup(repo => repo.ReportJob(It.IsAny<Report>()))
                .ReturnsAsync(-1);

            // Act
            var result = await _postJobService.ReportJob(reportCreateDto, invalidUserId);

            // Assert
            Xunit.Assert.Equal(-1, result);
            _mockPostJobRepository.Verify(repo => repo.ReportJob(It.Is<Report>(
                r => r.JobSeekerId == invalidUserId
            )), Times.Once);
        }

        #endregion

        // Các region test khác cho các method khác của PostJobService
    }
}
