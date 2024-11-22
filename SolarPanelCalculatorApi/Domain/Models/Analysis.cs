using System;
using System.ComponentModel.DataAnnotations;

namespace SolarPanelCalculatorApi.Domain.Models
{
    public class Analysis
    {
        public long Id { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        public double TotalConsumption { get; set; }

        [Required]
        [Range(0, 24)]
        public int SunlightHours { get; set; }

        [Required]
        public string Result { get; set; }

        [Required]
        public string AppliancesJson { get; set; }

        public long UserId { get; set; }
        public User User { get; set; }
    }
}
