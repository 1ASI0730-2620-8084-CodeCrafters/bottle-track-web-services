using BottleTrack.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BottleTrack.API.Tests.Integration;

public class ApiFactory : WebApplicationFactory<Program>
{
    private readonly SqliteConnection connection;

    public ApiFactory()
    {
        connection = new SqliteConnection("DataSource=:memory:");
        connection.Open();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureAppConfiguration((context, configuration) =>
        {
            Dictionary<string, string?> settings = new Dictionary<string, string?>();
            settings["Jwt:Secret"] = "integration-tests-secret-key-please-change-0123456789";
            settings["Jwt:Issuer"] = "BottleTrack.API";
            settings["Jwt:Audience"] = "BottleTrack.Client";
            settings["Jwt:ExpiresInDays"] = "1";
            configuration.AddInMemoryCollection(settings);
        });

        builder.ConfigureServices(services =>
        {
            List<ServiceDescriptor> toRemove = services.Where(service =>
                service.ServiceType == typeof(DbContextOptions<AppDbContext>) ||
                service.ServiceType == typeof(DbContextOptions) ||
                service.ServiceType == typeof(AppDbContext) ||
                (service.ServiceType.FullName != null &&
                 service.ServiceType.FullName.Contains("IDbContextOptionsConfiguration"))).ToList();

            foreach (ServiceDescriptor descriptor in toRemove)
                services.Remove(descriptor);

            services.AddDbContext<AppDbContext>(options => options.UseSqlite(connection));
        });
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        IHost host = base.CreateHost(builder);

        using (IServiceScope scope = host.Services.CreateScope())
        {
            AppDbContext context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            context.Database.EnsureCreated();
        }

        return host;
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        if (disposing)
            connection.Dispose();
    }
}
