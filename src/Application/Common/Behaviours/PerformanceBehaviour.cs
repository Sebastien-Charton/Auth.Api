using System.Diagnostics;
using Auth.Api.Application.Common.Interfaces;
using Auth.Api.Application.Common.Interfaces.Identity.Services;
using Microsoft.Extensions.Logging;

namespace Auth.Api.Application.Common.Behaviours;

public class PerformanceBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    private readonly ILogger<TRequest> _logger;
    private readonly Stopwatch _timer;
    private readonly IUser _user;
    private readonly IUserManagerService _userManagerService;

    public PerformanceBehaviour(
        ILogger<TRequest> logger,
        IUser user,
        IUserManagerService userManagerService)
    {
        _timer = new Stopwatch();

        _logger = logger;
        _user = user;
        _userManagerService = userManagerService;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        _timer.Start();

        TResponse response = await next();

        _timer.Stop();

        long elapsedMilliseconds = _timer.ElapsedMilliseconds;

        if (elapsedMilliseconds > 500)
        {
            string requestName = typeof(TRequest).Name;
            Guid? userId = _user.Id ?? null;
            string? userName = string.Empty;

            if (userId.HasValue)
            {
                userName = await _userManagerService.GetUserNameAsync(userId.Value);
            }

            _logger.LogWarning(
                "CleanArchitecture Long Running Request: {Name} ({ElapsedMilliseconds} milliseconds) {@UserId} {@UserName} {@Request}",
                requestName, elapsedMilliseconds, userId, userName, request);
        }

        return response;
    }
}
