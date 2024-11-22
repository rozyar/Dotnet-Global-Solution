using SolarPanelCalculatorApi.Domain.Models;
using SolarPanelCalculatorApi.Infrastructure.Data;

public static class SeedData
{
    public static async Task Initialize(AppDbContext context)
    {
        if (!context.Users.Any())
        {

            var user = new User
            {
                Name = "Demo User",
                Email = "demo@example.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123")
            };
            context.Users.Add(user);

            var appliances = new List<Appliance>
            {
                new Appliance { ApplianceName = "Refrigerator", PowerConsumption = 150, User = user },
                new Appliance { ApplianceName = "Air Conditioner", PowerConsumption = 2000, User = user }
            };
            context.Appliances.AddRange(appliances);


            var analyses = new List<Analysis>
            {
                new Analysis
                {
                    TotalConsumption = 2.5,
                    SunlightHours = 5,
                    Result = "Requires 10 panels",
                    AppliancesJson = "[{\"ApplianceName\":\"Refrigerator\",\"PowerConsumption\":150},{\"ApplianceName\":\"Air Conditioner\",\"PowerConsumption\":2000}]",
                    User = user
                },
                new Analysis
                {
                    TotalConsumption = 3.0,
                    SunlightHours = 6,
                    Result = "Requires 12 panels",
                    AppliancesJson = "[{\"ApplianceName\":\"Refrigerator\",\"PowerConsumption\":150}]",
                    User = user
                }
            };
            context.Analyses.AddRange(analyses);


            await context.SaveChangesAsync();
        }
    }
}
