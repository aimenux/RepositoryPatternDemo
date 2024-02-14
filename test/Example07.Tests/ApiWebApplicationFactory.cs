using Example07.Infrastructure;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;

namespace Example07.Tests;

internal class ApiWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment(Extensions.ContinuousIntegrationEnvironmentName);
        
        builder.ConfigureAppConfiguration((_, configBuilder) =>
        {
        });

        builder.ConfigureTestServices(services =>
        {
            services.AddDbContext<BookDbContext>(options =>
            {
                options.UseInMemoryDatabase("IntegrationTestsDB");
            });
            
            var serviceProvider = services.BuildServiceProvider();
            using var scope = serviceProvider.CreateScope();
            var scopedServiceProvider = scope.ServiceProvider;
            var context = scopedServiceProvider.GetRequiredService<BookDbContext>();
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
        });
    }
}