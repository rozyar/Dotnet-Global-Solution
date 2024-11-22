using System.ComponentModel.DataAnnotations;

namespace SolarPanelCalculatorApi.Domain.Models
{
    public class Appliance
    {
        public long Id { get; set; }

        [Required]
        public string ApplianceName { get; set; }

        [Required]
        public double PowerConsumption { get; set; }

        public long UserId { get; set; }
        public User User { get; set; }
    }
}
