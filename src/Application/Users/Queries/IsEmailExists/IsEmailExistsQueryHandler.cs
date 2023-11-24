using Auth.Api.Application.Common.Interfaces.Identity.Services;

namespace Auth.Api.Application.Users.Queries.IsEmailExists;

public record IsEmailExistsQuery : IRequest<bool>
{
    public string Email { get; set; } = null!;
}

public class IsEmailExistsQueryHandler : IRequestHandler<IsEmailExistsQuery, bool>
{
    private readonly IUserManagerService _userManagerService;

    public IsEmailExistsQueryHandler(IUserManagerService userManagerService)
    {
        _userManagerService = userManagerService;
    }

    public async Task<bool> Handle(IsEmailExistsQuery request, CancellationToken cancellationToken)
    {
        var isUserEmailExists = await _userManagerService.IsEmailExists(request.Email);

        return isUserEmailExists;
    }
}
