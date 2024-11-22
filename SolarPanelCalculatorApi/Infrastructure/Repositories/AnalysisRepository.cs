using Microsoft.EntityFrameworkCore;
using SolarPanelCalculatorApi.Domain.Interfaces;
using SolarPanelCalculatorApi.Domain.Models;
using SolarPanelCalculatorApi.Infrastructure.Data;

namespace SolarPanelCalculatorApi.Infrastructure.Repositories
{
    public class AnalysisRepository : Repository<Analysis>, IAnalysisRepository
    {
        public AnalysisRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<Analysis>> GetByUserIdAsync(long userId)
        {
            return await _context.Analyses
                .Where(a => a.UserId == userId)
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();
        }
    }
}
