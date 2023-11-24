using Auth.Api.Application.Common.Interfaces.Identity.Services;

namespace Auth.Api.Application.Users.Queries.IsUserNameExists;

public record IsUserNameExistsQuery : IRequest<bool>
{
    public string UserName { get; set; } = null!;
}

public class IsUserNameExistsQueryHandler : IRequestHandler<IsUserNameExistsQuery, bool>
{
    private readonly IUserManagerService _userManagerService;

    public IsUserNameExistsQueryHandler(IUserManagerService userManagerService)
    {
        _userManagerService = userManagerService;
    }

    public async Task<bool> Handle(IsUserNameExistsQuery request, CancellationToken cancellationToken)
    {
        var isUserUserNameExists = await _userManagerService.IsUserNameExists(request.UserName);

        return isUserUserNameExists;
    }
}
