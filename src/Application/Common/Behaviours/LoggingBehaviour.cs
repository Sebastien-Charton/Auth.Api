using Auth.Api.Application.Common.Interfaces;
using Auth.Api.Application.Common.Interfaces.Identity.Services;
using MediatR.Pipeline;
using Microsoft.Extensions.Logging;

namespace Auth.Api.Application.Common.Behaviours;

public class LoggingBehaviour<TRequest> : IRequestPreProcessor<TRequest> where TRequest : notnull
{
    private readonly ILogger _logger;
    private readonly IUser _user;
    private readonly IUserManagerService _userManagerService;

    public LoggingBehaviour(ILogger<TRequest> logger, IUser user, IUserManagerService userManagerService)
    {
        _logger = logger;
        _user = user;
        _userManagerService = userManagerService;
    }

    public Task Process(TRequest request, CancellationToken cancellationToken)
    {
        string requestName = typeof(TRequest).Name;
        
        _logger.LogInformation("CleanArchitecture Request: {Name} {@UserId} {@UserName} {@Request}",
            requestName, _user.Id, _user.UserName, request);
        return Task.CompletedTask;
    }
}
