using System.Reflection;
using Auth.Api.Application.Common.Behaviours;
using Auth.Api.Application.Common.Options;
using Auth.Api.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Auth.Api.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(AuthorizationBehaviour<,>));
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviour<,>));
        });

        services
            .AddOptionsWithValidateOnStart<FeatureOptions>()
            .Configure(featureOptions =>
                configuration.Bind(nameof(FeatureOptions), featureOptions))
            .ValidateOnStart();

        services
            .AddOptionsWithValidateOnStart<RefreshTokenOptions>()
            .Configure(refreshTokenOptions =>
            {
                configuration.Bind(nameof(RefreshTokenOptions), refreshTokenOptions);
            })
            .ValidateOnStart();

        return services;
    }
}
