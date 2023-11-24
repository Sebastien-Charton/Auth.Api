using System.Data.Common;
using Auth.Api.Application.Common.Interfaces.Identity.Services;
using Auth.Api.Infrastructure.Data;
using Auth.Api.Shared.Tests;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Auth.Api.Web.IntegrationTests;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly DbConnection _connection;
    private readonly ServiceDescriptor[] _serviceDescriptors;

    public CustomWebApplicationFactory(DbConnection connection, ServiceDescriptor[] serviceDescriptors)
    {
        _connection = connection;
        _serviceDescriptors = serviceDescriptors;
    }

    public Guid DefaultUserId { get; private set; }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        System.Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Test");
        var environment = System.Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

        IConfigurationRoot configuration = new ConfigurationBuilder()
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

            var userManager = services.BuildServiceProvider().GetRequiredService<IUserManagerService>();

            var userName = new Faker().Internet.UserName();
            var password = new Faker().Internet.GeneratePassword();
            var email = new Faker().Internet.Email();

            var userCreatedResponse = userManager
                .CreateUserAsync(userName, password, email, null)
                .GetAwaiter()
                .GetResult();

            DefaultUserId = userCreatedResponse.userId;

            services.Configure<TestAuthHandlerOptions>(options => options.DefaultUserId = DefaultUserId);

            services.AddAuthentication(TestAuthHandler.AuthenticationScheme)
                .AddScheme<TestAuthHandlerOptions, TestAuthHandler>(TestAuthHandler.AuthenticationScheme,
                    options => { });

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
