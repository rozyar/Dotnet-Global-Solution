using SolarPanelCalculatorApi.Domain.Interfaces;
using SolarPanelCalculatorApi.Infrastructure.Data;

namespace SolarPanelCalculatorApi.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        public IUserRepository Users { get; private set; }
        public IApplianceRepository Appliances { get; private set; }
        public IAnalysisRepository Analyses { get; private set; }

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            Users = new UserRepository(context);
            Appliances = new ApplianceRepository(context);
            Analyses = new AnalysisRepository(context);
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
