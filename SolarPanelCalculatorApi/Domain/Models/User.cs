using System.ComponentModel.DataAnnotations;

namespace SolarPanelCalculatorApi.Domain.Models
{
    public class User
    {
        public long Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        public ICollection<Appliance> Appliances { get; set; }
        public ICollection<Analysis> Analyses { get; set; }
    }
}
