using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;
using SolarPanelCalculatorApi.Models;
using SolarPanelCalculatorApi.Infrastructure.Data;
using SolarPanelCalculatorApi.Infrastructure.Repositories;

namespace SolarPanelCalculatorApi.Tests.Repositories
{
    public class UserRepositoryTests
    {
        private readonly AppDbContext _context;
        private readonly UserRepository _userRepository;

        public UserRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            _context = new AppDbContext(options);
            _userRepository = new UserRepository(_context);
        }

        [Fact]
        public async Task GetByEmailAsync_WithExistingEmail_ReturnsUser()
        {
            // Arrange
            var email = "test@example.com";
            var user = new User { Email = email, Name = "Test User", PasswordHash = "hashedpassword" };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Act
            var result = await _userRepository.GetByEmailAsync(email);

            // Assert
            result.Should().NotBeNull();
            result.Email.Should().Be(email);
        }

        [Fact]
        public async Task GetByEmailAsync_WithNonExistingEmail_ReturnsNull()
        {
            // Arrange
            var email = "nonexisting@example.com";

            // Act
            var result = await _userRepository.GetByEmailAsync(email);

            // Assert
            result.Should().BeNull();
        }
    }
}
