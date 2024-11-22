using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.Configuration;
using SolarPanelCalculatorApi.Domain.Models;
using SolarPanelCalculatorApi.Domain.Interfaces;

namespace SolarPanelCalculatorApi.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public UserService(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }

        public async Task<User> Authenticate(string email, string password)
        {
            var user = await _unitOfWork.Users.GetByEmailAsync(email);
            if (user == null || !VerifyPassword(password, user.PasswordHash))
                return null;

            return user;
        }

        public async Task<User> Register(User user, string password)
        {
            if (await _unitOfWork.Users.GetByEmailAsync(user.Email) != null)
                throw new ApplicationException("Email already exists");

            user.PasswordHash = HashPassword(password);
            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.CompleteAsync();

            return user;
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            return await _unitOfWork.Users.GetAllAsync();
        }

        public async Task<User> GetById(long id)
        {
            return await _unitOfWork.Users.GetAsync(id);
        }

        // Password hashing methods
        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        private bool VerifyPassword(string password, string storedHash)
        {
            return BCrypt.Net.BCrypt.Verify(password, storedHash);
        }
    }
}