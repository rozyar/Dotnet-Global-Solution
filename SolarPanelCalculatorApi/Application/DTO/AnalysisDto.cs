using System;
using System.ComponentModel.DataAnnotations;

namespace SolarPanelCalculatorApi.Application.DTO
{
    public class AnalysisDto
    {
        public long Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public string FormattedDate => CreatedAt == DateTime.MinValue
        ? "Data Indisponível"
        : CreatedAt.ToString("dd/MM/yyyy HH:mm");

        public double TotalConsumption { get; set; }

        [Required]
        [Range(0, 24)]
        public int SunlightHours { get; set; }

        public List<ApplianceDto>? Appliances { get; set; }

        public string? Result { get; set; }
    }
}