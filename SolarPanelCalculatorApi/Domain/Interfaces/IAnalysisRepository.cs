using SolarPanelCalculatorApi.Domain.Models;

namespace SolarPanelCalculatorApi.Domain.Interfaces
{
    public interface IAnalysisRepository : IRepository<Analysis>
    {
        Task<IEnumerable<Analysis>> GetByUserIdAsync(long userId);
    }
}
