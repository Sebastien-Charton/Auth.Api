using Auth.Api.Application.Common.Interfaces;
using Auth.Api.Application.Common.Interfaces.Identity.Services;
using Auth.Api.Application.Common.Interfaces.ServiceAgents;
using Auth.Api.Application.Common.Interfaces.Services;
using Auth.Api.Domain.Constants;
using Auth.Api.Infrastructure.Data;
using Auth.Api.Infrastructure.Data.Interceptors;
using Auth.Api.Infrastructure.Identity.Models;
using Auth.Api.Infrastructure.Identity.Services;
using Auth.Api.Infrastructure.Options;
using Auth.Api.Infrastructure.ServiceAgents;
using Auth.Api.Infrastructure.Services;
using Auth.Api.Infrastructure.Services.HtmlGeneration;
using Mailjet.Client;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SendGrid.Extensions.DependencyInjection;
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

        // Inject options

        services
            .AddOptionsWithValidateOnStart<JwtOptions>()
            .Configure(jwtOptions => configuration.Bind(nameof(JwtOptions), jwtOptions))
            .Validate(x => x.Audience is not null, $"{nameof(JwtOptions.Audience)} is null")
            .Validate(x => x.Issuer is not null, $"{nameof(JwtOptions.Issuer)} is null")
            .Validate(x => x.SecurityKey is not null, $"{nameof(JwtOptions.SecurityKey)} is null")
            .Validate(x => x.ExpiryInDays > 0, $"{nameof(JwtOptions.ExpiryInDays)} is less than 1")
            .ValidateOnStart();

        services
            .AddOptionsWithValidateOnStart<SendGridOptions>()
            .Configure(sendGridOptions =>
                configuration.Bind(nameof(SendGridOptions), sendGridOptions))
            .Validate(x => x.ApiKey is not null, $"{nameof(SendGridOptions.ApiKey)} is null")
            .Validate(x => x.FromEmail is not null, $"{nameof(SendGridOptions.FromEmail)} is null")
            .Validate(x => x.FromName is not null, $"{nameof(SendGridOptions.FromName)} is null")
            .ValidateOnStart();

        services
            .AddOptionsWithValidateOnStart<MailJetOptions>()
            .Configure(mailJetOptions =>
                configuration.Bind(nameof(MailJetOptions), mailJetOptions))
            .Validate(x => x.ApiKey is not null, $"{nameof(MailJetOptions.ApiKey)} is null")
            .Validate(x => x.ApiSecret is not null, $"{nameof(MailJetOptions.ApiSecret)} is null")
            .Validate(x => x.FromEmail is not null, $"{nameof(MailJetOptions.FromEmail)} is null")
            .Validate(x => x.FromName is not null, $"{nameof(MailJetOptions.FromName)} is null")
            .ValidateOnStart();

        services
            .AddOptionsWithValidateOnStart<IdentityOptions>()
            .Configure(identityOptions =>
                configuration.Bind(nameof(IdentityOptions), identityOptions))
            .Validate(x => x.Password is not null, $"{nameof(IdentityOptions.Password)} is null")
            .Validate(x => x.Password.RequiredLength > 0,
                $"{nameof(IdentityOptions.Password.RequiredLength)} is less than 1")
            .Validate(x => x.Password.RequiredUniqueChars > 0,
                $"{nameof(IdentityOptions.Password.RequiredUniqueChars)} is less than 1")
            .Validate(x => x.Lockout is not null, $"{nameof(IdentityOptions.Lockout)} is null")
            .Validate(x => x.Lockout.DefaultLockoutTimeSpanInMs > 0,
                $"{nameof(IdentityOptions.Lockout.DefaultLockoutTimeSpanInMs)} is less than 1")
            .Validate(x => x.Lockout.MaxFailedAccessAttempts > 0, $"{nameof(IdentityOptions.Lockout)} is less than 1")
            .Validate(x => x.User is not null, $"{nameof(IdentityOptions.User)} is null")
            .Validate(x => x.User.AllowedUserNameCharacters is not null,
                $"{nameof(IdentityOptions.User.AllowedUserNameCharacters)} is less than 1")
            .Validate(x => x.SignIn is not null, $"{nameof(IdentityOptions.SignIn)} is null")
            .ValidateOnStart();

        // Inject service
        services.AddTransient<IUserManagerService, UserManagerService>();
        services.AddTransient<ISignInService, SignInService>();
        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<IMailServiceAgent, MailJetServiceAgent>();
        services.AddScoped<IHtmlGenerator, DotLiquidHtmlGenerator>();

        var serviceProvider = services.BuildServiceProvider();
        var mailJetOptions = serviceProvider.GetRequiredService<IOptions<MailJetOptions>>();

        var jwt = serviceProvider.GetRequiredService<IOptions<JwtOptions>>();

        services.AddHttpClient<IMailjetClient, MailjetClient>(client =>
        {
            client.SetDefaultSettings();

            client.UseBasicAuthentication(mailJetOptions.Value.ApiKey, mailJetOptions.Value.ApiSecret);
        });

        services
            .AddIdentityCore<ApplicationUser>()
            .AddRoles<ApplicationRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddApiEndpoints();

        var identityOptions = serviceProvider.GetRequiredService<IOptions<IdentityOptions>>();

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

        var mailOptions = services.BuildServiceProvider().GetRequiredService<IOptions<SendGridOptions>>();

        services.AddSendGrid(options =>
        {
            options.ApiKey = mailOptions.Value.ApiKey;
        });

        return services;
    }
}
