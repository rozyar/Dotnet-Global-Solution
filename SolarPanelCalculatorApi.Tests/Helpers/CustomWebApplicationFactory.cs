using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Moq; // Para usar o Mock
using Microsoft.AspNetCore.Builder;
using SolarPanelCalculatorApi.Infrastructure.Data;
using SolarPanelCalculatorApi.Infrastructure.Repositories;
using SolarPanelCalculatorApi.Application.Services;
using SolarPanelCalculatorApi.Domain.Interfaces;

namespace SolarPanelCalculatorApi.Tests.Helpers
{
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Development");

            builder.ConfigureAppConfiguration((context, config) =>
            {
                var testConfig = new Dictionary<string, string>
                {
                    { "Jwt:SecretKey", "TestSecretKey123" },
                    { "OpenAI:ApiKey", "TestApiKey" }
                };
                config.AddInMemoryCollection(testConfig);
            });

            builder.ConfigureServices(services =>
            {
               
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));
                if (descriptor != null)
                    services.Remove(descriptor);
                services.AddControllers();
              
                services.AddDbContext<AppDbContext>(options =>
                {
                    options.UseInMemoryDatabase("TestDb");
                });

              
                services.AddScoped<IUserRepository, UserRepository>();
                services.AddScoped<IApplianceRepository, ApplianceRepository>();
                services.AddScoped<IAnalysisRepository, AnalysisRepository>();

              
                services.AddScoped<IUnitOfWork, UnitOfWork>();

                
                var aiServiceMock = new Mock<IAIService>();
                aiServiceMock.Setup(a => a.CalculateSolarPanels(It.IsAny<double>(), It.IsAny<int>()))
              .ReturnsAsync("Mocked AI Service Response");
                services.AddSingleton<IAIService>(aiServiceMock.Object);

                
                services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

               
                services.AddScoped<IAnalysisService, AnalysisService>();
                services.AddScoped<IApplianceService, ApplianceService>();
                services.AddScoped<IUserService, UserService>();

                var sp = services.BuildServiceProvider();

                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<AppDbContext>();
                    var mapper = scopedServices.GetRequiredService<IMapper>();

                    // Garantir que o banco de dados foi criado
                    db.Database.EnsureCreated();

                    // Aplicar seed data
                    SeedData.Initialize(db).Wait();
                }
            });

            builder.Configure(app =>
            {
                // Middleware para captura de erros detalhados
                app.UseDeveloperExceptionPage();

                app.UseRouting();
                app.UseAuthentication();
                app.UseAuthorization();
                app.UseCors("AllowAllOrigins");
                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                });
            });
        }
    }
}
