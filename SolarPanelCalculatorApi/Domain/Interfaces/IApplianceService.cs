using SolarPanelCalculatorApi.Domain.Models;

namespace SolarPanelCalculatorApi.Domain.Interfaces
{
    public interface IApplianceService
    {
        Task<IEnumerable<Appliance>> GetAppliancesByUserId(long userId);
        Task<Appliance> GetApplianceById(long id);
        Task<IEnumerable<Appliance>> GetPagedAppliancesByUserId(long userId, int page, int pageSize);
        Task AddAppliance(Appliance appliance);
        Task UpdateAppliance(Appliance appliance);
        Task DeleteAppliance(long id);
    }
}
