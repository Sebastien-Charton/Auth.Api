using Auth.Api.Application.Common.Interfaces;
using Auth.Api.Application.Common.Interfaces.Identity.Services;

namespace Auth.Api.Application.Users.Queries.IsEmailConfirmed;

public record IsEmailConfirmedQuery : IRequest<bool>
{
}

public class ValidateEmailQueryHandler : IRequestHandler<IsEmailConfirmedQuery, bool>
{
    private readonly IUser _user;
    private readonly IUserManagerService _userManagerService;

    public ValidateEmailQueryHandler(IUserManagerService userManagerService, IUser user)
    {
        _userManagerService = userManagerService;
        _user = user;
    }

    public async Task<bool> Handle(IsEmailConfirmedQuery request, CancellationToken cancellationToken)
    {
        var user = await _userManagerService.GetUserByIdAsync(_user.Id!.Value);

        Guard.Against.NotFound(nameof(user), user);

        return await _userManagerService.IsEmailConfirmedAsync(_user.Id!.Value);
    }
}
