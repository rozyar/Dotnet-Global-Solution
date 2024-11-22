using SolarPanelCalculatorApi.Domain.Interfaces;
using SolarPanelCalculatorApi.Domain.Models;

namespace SolarPanelCalculatorApi.Application.Services
{
    public class ApplianceService : IApplianceService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ApplianceService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Appliance>> GetPagedAppliancesByUserId(long userId, int page, int pageSize)
        {
            var appliances = await _unitOfWork.Appliances.GetByUserIdAsync(userId);
            return appliances.Skip(page * pageSize).Take(pageSize);
        }

        public async Task<IEnumerable<Appliance>> GetAppliancesByUserId(long userId)
        {
            return await _unitOfWork.Appliances.GetByUserIdAsync(userId);
        }

        public async Task<Appliance> GetApplianceById(long id)
        {
            return await _unitOfWork.Appliances.GetAsync(id);
        }

        public async Task AddAppliance(Appliance appliance)
        {
            if (appliance.PowerConsumption <= 0)
                throw new ArgumentException("Power consumption must be positive.");

            await _unitOfWork.Appliances.AddAsync(appliance);
            await _unitOfWork.CompleteAsync();
        }


        public async Task UpdateAppliance(Appliance appliance)
        {
            _unitOfWork.Appliances.Update(appliance);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteAppliance(long id)
        {
            var appliance = await _unitOfWork.Appliances.GetAsync(id);
            if (appliance != null)
            {
                _unitOfWork.Appliances.Remove(appliance);
                await _unitOfWork.CompleteAsync();
            }
        }
    }
}
