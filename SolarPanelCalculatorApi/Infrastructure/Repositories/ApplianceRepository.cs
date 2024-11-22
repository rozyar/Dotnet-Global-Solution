using Microsoft.EntityFrameworkCore;
using SolarPanelCalculatorApi.Domain.Interfaces;
using SolarPanelCalculatorApi.Domain.Models;
using SolarPanelCalculatorApi.Infrastructure.Data;

namespace SolarPanelCalculatorApi.Infrastructure.Repositories
{
    public class ApplianceRepository : Repository<Appliance>, IApplianceRepository
    {
        public ApplianceRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<Appliance>> GetByUserIdAsync(long userId)
        {
            return await _context.Appliances
                .Where(a => a.UserId == userId)
                .ToListAsync();
        }

    }
}
