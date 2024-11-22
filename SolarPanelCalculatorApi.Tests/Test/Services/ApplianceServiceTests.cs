using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Moq;
using FluentAssertions;
using SolarPanelCalculatorApi.Application.Services;
using SolarPanelCalculatorApi.Domain.Models;
using SolarPanelCalculatorApi.Domain.Interfaces;

namespace SolarPanelCalculatorApi.Tests.Services
{
    public class ApplianceServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IApplianceRepository> _applianceRepositoryMock;
        private readonly ApplianceService _applianceService;

        public ApplianceServiceTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _applianceRepositoryMock = new Mock<IApplianceRepository>();
            _unitOfWorkMock.Setup(u => u.Appliances).Returns(_applianceRepositoryMock.Object);
            _applianceService = new ApplianceService(_unitOfWorkMock.Object);
        }

        [Fact]
        public async Task GetAppliancesByUserId_ReturnsAppliances()
        {
            // Arrange
            var userId = 1L;
            var appliances = new List<Appliance> { new Appliance { Id = 1, UserId = userId } };
            _applianceRepositoryMock.Setup(r => r.GetByUserIdAsync(userId)).ReturnsAsync(appliances);

            // Act
            var result = await _applianceService.GetAppliancesByUserId(userId);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(1);
        }

    }
}
