using System.Data.Common;
using Auth.Api.Infrastructure.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Auth.Api.Infrastructure.IntegrationTests;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly DbConnection _connection;
    private readonly ServiceDescriptor[] _serviceDescriptors;

    public CustomWebApplicationFactory(DbConnection connection, ServiceDescriptor[] serviceDescriptors)
    {
        _connection = connection;
        _serviceDescriptors = serviceDescriptors;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Test");
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

        new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile($"appsettings.{environment}.secrets.json", false, true)
            .AddJsonFile($"appsettings.{environment}.json", false, true)
            .Build();

        builder.ConfigureTestServices(services =>
        {
            services
                .RemoveAll<DbContextOptions<ApplicationDbContext>>()
                .AddDbContext<ApplicationDbContext>((sp, options) =>
                {
                    options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
                    options.UseNpgsql(_connection);
                });

            ConfigureTestServices(services);
        });
    }

    private void ConfigureTestServices(IServiceCollection serviceCollection)
    {
        foreach (var mock in _serviceDescriptors)
        {
            serviceCollection.Replace(mock);
        }
    }
}
