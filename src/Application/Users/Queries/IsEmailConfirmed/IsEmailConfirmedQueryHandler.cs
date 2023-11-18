using Auth.Api.Application.Common.Interfaces.Identity.Services;

namespace Auth.Api.Application.Users.Queries.IsEmailConfirmed;

public record IsEmailConfirmedQuery : IRequest<bool>
{
    public Guid UserId { get; set; }
}

public class ValidateEmailQueryHandler : IRequestHandler<IsEmailConfirmedQuery, bool>
{
    private readonly IUserManagerService _userManagerService;

    public ValidateEmailQueryHandler(IUserManagerService userManagerService)
    {
        _userManagerService = userManagerService;
    }

    public async Task<bool> Handle(IsEmailConfirmedQuery request, CancellationToken cancellationToken)
    {
        var user = await _userManagerService.GetUserByIdAsync(request.UserId);

        Guard.Against.NotFound(nameof(user), user);

        return await _userManagerService.IsEmailConfirmed(request.UserId);
    }
}
