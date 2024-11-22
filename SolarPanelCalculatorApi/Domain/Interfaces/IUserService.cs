using SolarPanelCalculatorApi.Domain.Models;

namespace SolarPanelCalculatorApi.Domain.Interfaces
{
    public interface IUserService
    {
        Task<User> Authenticate(string email, string password);
        Task<User> Register(User user, string password);
        Task<IEnumerable<User>> GetAll();
        Task<User> GetById(long id);
    }
}
