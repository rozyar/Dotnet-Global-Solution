using System;
using System.Threading.Tasks;
using Xunit;
using Moq;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using SolarPanelCalculatorApi.Application.Services;
using SolarPanelCalculatorApi.Domain.Models;
using SolarPanelCalculatorApi.Domain.Interfaces;

namespace SolarPanelCalculatorApi.Tests.Services
{
    public class UserServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly UserService _userService;

        public UserServiceTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _configurationMock = new Mock<IConfiguration>();

            // Simula as dependências de _unitOfWork e _configuration
            _unitOfWorkMock.Setup(u => u.Users).Returns(_userRepositoryMock.Object);
            _configurationMock.Setup(c => c["Jwt:SecretKey"]).Returns("TestSecretKey123");

            // Instancia UserService com os mocks
            _userService = new UserService(_unitOfWorkMock.Object, _configurationMock.Object);
        }

        [Fact]
        public async Task Authenticate_WithValidCredentials_ReturnsUser()
        {
            // Arrange
            var email = "test@example.com";
            var password = "password";
            var user = new User { Email = email, PasswordHash = BCrypt.Net.BCrypt.HashPassword(password) };
            _userRepositoryMock.Setup(r => r.GetByEmailAsync(email)).ReturnsAsync(user);

            // Act
            var result = await _userService.Authenticate(email, password);

            // Assert
            result.Should().NotBeNull();
            result.Email.Should().Be(email);
        }

        [Fact]
        public async Task Authenticate_WithInvalidCredentials_ReturnsNull()
        {
            // Arrange
            var email = "test@example.com";
            var password = "wrongpassword";
            var user = new User { Email = email, PasswordHash = BCrypt.Net.BCrypt.HashPassword("password") };
            _userRepositoryMock.Setup(r => r.GetByEmailAsync(email)).ReturnsAsync(user);

            // Act
            var result = await _userService.Authenticate(email, password);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task Register_WithExistingEmail_ThrowsException()
        {
            // Arrange
            var user = new User { Email = "existing@example.com", Name = "Existing User" };
            var password = "Password123!";
            _userRepositoryMock.Setup(r => r.GetByEmailAsync(user.Email)).ReturnsAsync(user);

            // Act
            Func<Task> action = async () => await _userService.Register(user, password);

            // Assert
            await action.Should().ThrowAsync<ApplicationException>().WithMessage("Email already exists");
        }

        [Fact]
        public async Task Register_WithValidData_ReturnsUser()
        {
            // Arrange
            var user = new User { Email = "newuser@example.com", Name = "New User" };
            var password = "Password123!";
            _userRepositoryMock.Setup(r => r.GetByEmailAsync(user.Email)).ReturnsAsync((User)null);

            // Act
            var result = await _userService.Register(user, password);

            // Assert
            result.Should().NotBeNull();
            result.Email.Should().Be(user.Email);
            _unitOfWorkMock.Verify(u => u.CompleteAsync(), Times.Once);
        }
    }
}
