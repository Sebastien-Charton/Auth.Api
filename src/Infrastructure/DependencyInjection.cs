using Auth.Api.Application.Common.Interfaces;
using Auth.Api.Application.Common.Interfaces.Identity.Services;
using Auth.Api.Application.Common.Interfaces.Services;
using Auth.Api.Domain.Constants;
using Auth.Api.Infrastructure.Data;
using Auth.Api.Infrastructure.Data.Interceptors;
using Auth.Api.Infrastructure.Identity.Models;
using Auth.Api.Infrastructure.Identity.Services;
using Auth.Api.Infrastructure.Options;
using Auth.Api.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using IdentityOptions = Auth.Api.Infrastructure.Options.IdentityOptions;

namespace Auth.Api.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        string? connectionString = configuration.GetConnectionString("DefaultConnection");

        Guard.Against.Null(connectionString, message: "Connection string 'DefaultConnection' not found.");

        services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
        services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();

        services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
            options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());

            options.UseNpgsql(connectionString, options =>
            {
            });
        });

        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

        services.AddScoped<ApplicationDbContextInitialiser>();

        services.AddAuthentication()
            .AddBearerToken(IdentityConstants.BearerScheme, options =>
            {
            });

        services.AddAuthorizationBuilder();

        services.AddSingleton(TimeProvider.System);

        // Inject service
        services.AddTransient<IUserManagerService, UserManagerService>();
        services.AddTransient<ISignInService, SignInService>();
        services.AddScoped<IJwtService, JwtService>();

        // Inject options
        services.Configure<JwtOptions>(jwtOptions => configuration.Bind(nameof(JwtOptions), jwtOptions));

        services.Configure<IdentityOptions>(identityOptions =>
            configuration.Bind(nameof(IdentityOptions), identityOptions));


        services
            .AddIdentityCore<ApplicationUser>()
            .AddRoles<ApplicationRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddApiEndpoints();

        var identityOptions = services.BuildServiceProvider().GetRequiredService<IOptions<IdentityOptions>>();

        services.Configure<Microsoft.AspNetCore.Identity.IdentityOptions>(options =>
        {
            // Password settings.
            options.Password.RequireDigit = identityOptions.Value.Password.RequireDigit;
            options.Password.RequireLowercase = identityOptions.Value.Password.RequireLowercase;
            options.Password.RequireNonAlphanumeric = identityOptions.Value.Password.RequireNonAlphanumeric;
            options.Password.RequireUppercase = identityOptions.Value.Password.RequireUppercase;
            options.Password.RequiredLength = identityOptions.Value.Password.RequiredLength;
            options.Password.RequiredUniqueChars = identityOptions.Value.Password.RequiredUniqueChars;

            // Lockout settings.
            options.Lockout.DefaultLockoutTimeSpan =
                TimeSpan.FromMilliseconds(identityOptions.Value.Lockout.DefaultLockoutTimeSpanInMs);
            options.Lockout.MaxFailedAccessAttempts = identityOptions.Value.Lockout.MaxFailedAccessAttempts;
            options.Lockout.AllowedForNewUsers = identityOptions.Value.Lockout.AllowedForNewUsers;

            // User settings.
            options.User.AllowedUserNameCharacters = identityOptions.Value.User.AllowedUserNameCharacters;
            options.User.RequireUniqueEmail = identityOptions.Value.User.RequireUniqueEmail;
        });

        services.AddAuthorization(options =>
            options.AddPolicy(Policies.CanPurge, policy => policy.RequireRole(Roles.Administrator)));

        return services;
    }
}
