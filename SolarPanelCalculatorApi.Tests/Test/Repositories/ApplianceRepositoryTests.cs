using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;
using SolarPanelCalculatorApi.Infrastructure.Data;
using SolarPanelCalculatorApi.Infrastructure.Repositories;
using SolarPanelCalculatorApi.Domain.Models;

namespace SolarPanelCalculatorApi.Tests.Repositories
{
    public class ApplianceRepositoryTests
    {
        private readonly AppDbContext _context;
        private readonly ApplianceRepository _applianceRepository;

        public ApplianceRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            _context = new AppDbContext(options);
            _applianceRepository = new ApplianceRepository(_context);
        }

        [Fact]
        public async Task GetByUserIdAsync_WithExistingUserId_ReturnsAppliances()
        {
            // Arrange
            var userId = 1L;
            var appliances = new List<Appliance>
            {
                new Appliance { ApplianceName = "Fridge", PowerConsumption = 150, UserId = userId },
                new Appliance { ApplianceName = "TV", PowerConsumption = 100, UserId = userId }
            };
            _context.Appliances.AddRange(appliances);
            await _context.SaveChangesAsync();

            // Act
            var result = await _applianceRepository.GetByUserIdAsync(userId);

            // Assert
            result.Should().HaveCount(2);
        }
    }
}
