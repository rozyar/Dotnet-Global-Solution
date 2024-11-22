using Microsoft.EntityFrameworkCore;
using SolarPanelCalculatorApi.Domain.Models;

namespace SolarPanelCalculatorApi.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Appliance> Appliances { get; set; }
        public DbSet<Analysis> Analyses { get; set; }
    }
}
