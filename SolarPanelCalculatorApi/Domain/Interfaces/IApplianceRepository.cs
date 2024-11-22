using SolarPanelCalculatorApi.Domain.Models;

namespace SolarPanelCalculatorApi.Domain.Interfaces
{
    public interface IApplianceRepository : IRepository<Appliance>
    {
        Task<IEnumerable<Appliance>> GetByUserIdAsync(long userId);
    }
}
