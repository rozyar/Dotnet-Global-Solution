public interface IAIService
{
    Task<string> CalculateSolarPanels(double totalConsumption, int sunlightHours);
}
