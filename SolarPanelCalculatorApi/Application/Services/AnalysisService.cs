using SolarPanelCalculatorApi.Domain.Interfaces;
using SolarPanelCalculatorApi.Domain.Models;
using System.Text.Json;

namespace SolarPanelCalculatorApi.Application.Services
{
    public class AnalysisService : IAnalysisService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAIService _aiService;

        public AnalysisService(IUnitOfWork unitOfWork, IAIService aiService)
        {
            _unitOfWork = unitOfWork;
            _aiService = aiService;
        }

        public async Task<IEnumerable<Analysis>> GetAnalysesByUserId(long userId)
        {
            return await _unitOfWork.Analyses.GetByUserIdAsync(userId);
        }

        public async Task<Analysis> GetAnalysisById(long id)
        {
            return await _unitOfWork.Analyses.GetAsync(id);
        }

        public async Task<Analysis> CreateAnalysis(Analysis analysis, IEnumerable<Appliance> appliances)
        {
            if (!appliances.Any())
                throw new ArgumentException("You need to add appliances before creating an analysis.");

            double totalConsumption = appliances.Sum(a => a.PowerConsumption) / 1000;

            if (analysis.SunlightHours <= 0 || analysis.SunlightHours > 24)
                throw new ArgumentException("Sunlight hours must be between 1 and 24.");

            string result = await _aiService.CalculateSolarPanels(totalConsumption, analysis.SunlightHours);

            analysis.TotalConsumption = totalConsumption;
            analysis.Result = result;
            analysis.CreatedAt = DateTime.UtcNow;

            analysis.AppliancesJson = JsonSerializer.Serialize(appliances.Select(a => new
            {
                a.Id,
                a.ApplianceName,
                a.PowerConsumption,

            }).ToList());

            await _unitOfWork.Analyses.AddAsync(analysis);
            await _unitOfWork.CompleteAsync();

            return analysis;
        }


        public async Task DeleteAnalysis(long id)
        {
            var analysis = await _unitOfWork.Analyses.GetAsync(id);
            if (analysis != null)
            {
                _unitOfWork.Analyses.Remove(analysis);
                await _unitOfWork.CompleteAsync();
            }
        }

        public async Task<IEnumerable<Analysis>> GetPagedAnalysesByUserId(long userId, int page, int pageSize)
        {
            var analyses = await _unitOfWork.Analyses.GetByUserIdAsync(userId);
            return analyses.Skip(page * pageSize).Take(pageSize);
        }

    }
}
