using System.ComponentModel.DataAnnotations;

namespace SolarPanelCalculatorApi.Application.DTO
{
    public class ApplianceDto
    {
        public long Id { get; set; }

        [Required]
        public string ApplianceName { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Power consumption must be positive")]
        public double PowerConsumption { get; set; }
    }
}
