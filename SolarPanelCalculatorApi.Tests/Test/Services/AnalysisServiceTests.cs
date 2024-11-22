using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Xunit;
using Microsoft.Extensions.Configuration;
using SolarPanelCalculatorApi.Application.Services;
using SolarPanelCalculatorApi.Domain.Models;
using SolarPanelCalculatorApi.Domain.Interfaces;

namespace SolarPanelCalculatorApi.Tests.Services
{
    public class AnalysisServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IAnalysisRepository> _analysisRepositoryMock;
        private readonly Mock<IAIService> _aiServiceMock;
        private readonly AnalysisService _analysisService;

        public AnalysisServiceTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _analysisRepositoryMock = new Mock<IAnalysisRepository>();
            _unitOfWorkMock.Setup(u => u.Analyses).Returns(_analysisRepositoryMock.Object);

            _aiServiceMock = new Mock<IAIService>();

            _analysisService = new AnalysisService(_unitOfWorkMock.Object, _aiServiceMock.Object);
        }

        [Fact]
        public async Task GetAnalysesByUserId_ReturnsAnalyses()
        {
            // Arrange
            var userId = 1L;
            var analyses = new List<Analysis>
            {
                new Analysis { Id = 1, UserId = userId, Result = "Result 1" },
                new Analysis { Id = 2, UserId = userId, Result = "Result 2" }
            };
            _analysisRepositoryMock.Setup(r => r.GetByUserIdAsync(userId)).ReturnsAsync(analyses);

            // Act
            var result = await _analysisService.GetAnalysesByUserId(userId);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
        }

        [Fact]
        public async Task CreateAnalysis_WithValidData_ReturnsAnalysis()
        {
            // Arrange
            var analysis = new Analysis { SunlightHours = 5, UserId = 1 };
            var appliances = new List<Appliance>
            {
                new Appliance { Id = 1, PowerConsumption = 1000 },
                new Appliance { Id = 2, PowerConsumption = 2000 }
            };

            _aiServiceMock.Setup(a => a.CalculateSolarPanels(It.IsAny<double>(), It.IsAny<int>()))
                          .ReturnsAsync("Requires 10 panels");

            // Act
            var result = await _analysisService.CreateAnalysis(analysis, appliances);

            // Assert
            result.Should().NotBeNull();
            result.Result.Should().Be("Requires 10 panels");
            _unitOfWorkMock.Verify(u => u.CompleteAsync(), Times.Once);
        }

        [Fact]
        public async Task CreateAnalysis_WithNoAppliances_ThrowsException()
        {
            // Arrange
            var analysis = new Analysis { SunlightHours = 5, UserId = 1 };
            var appliances = new List<Appliance>();

            // Act
            Func<Task> action = async () => await _analysisService.CreateAnalysis(analysis, appliances);

            // Assert
            await action.Should().ThrowAsync<ArgumentException>()
                .WithMessage("You need to add appliances before creating an analysis.");
        }

        [Fact]
        public async Task DeleteAnalysis_WithValidId_DeletesAnalysis()
        {
            // Arrange
            var analysisId = 1L;
            var analysis = new Analysis { Id = analysisId };
            _analysisRepositoryMock.Setup(r => r.GetAsync(analysisId)).ReturnsAsync(analysis);

            // Act
            await _analysisService.DeleteAnalysis(analysisId);

            // Assert
            _analysisRepositoryMock.Verify(r => r.Remove(analysis), Times.Once);
            _unitOfWorkMock.Verify(u => u.CompleteAsync(), Times.Once);
        }
    }
}
