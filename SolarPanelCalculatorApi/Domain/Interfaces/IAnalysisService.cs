using SolarPanelCalculatorApi.Domain.Models;

namespace SolarPanelCalculatorApi.Domain.Interfaces
{
    public interface IAnalysisService
    {
        Task<IEnumerable<Analysis>> GetAnalysesByUserId(long userId);
        Task<Analysis> GetAnalysisById(long id);
        Task<Analysis> CreateAnalysis(Analysis analysis, IEnumerable<Appliance> appliances);
        Task DeleteAnalysis(long id);
        Task<IEnumerable<Analysis>> GetPagedAnalysesByUserId(long userId, int page, int pageSize);
    }
}
