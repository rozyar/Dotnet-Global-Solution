using Microsoft.EntityFrameworkCore;
using SolarPanelCalculatorApi.Domain.Interfaces;
using SolarPanelCalculatorApi.Domain.Models;
using SolarPanelCalculatorApi.Infrastructure.Data;

namespace SolarPanelCalculatorApi.Infrastructure.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(AppDbContext context) : base(context) { }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}
