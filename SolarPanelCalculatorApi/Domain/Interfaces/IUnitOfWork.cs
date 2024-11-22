namespace SolarPanelCalculatorApi.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository Users { get; }
        IApplianceRepository Appliances { get; }
        IAnalysisRepository Analyses { get; }
        Task<int> CompleteAsync();
    }
}
