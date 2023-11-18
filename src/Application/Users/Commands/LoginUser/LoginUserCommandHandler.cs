using Auth.Api.Application.Common.Interfaces.Identity.Services;
using Auth.Api.Application.Common.Interfaces.Services;
using Auth.Api.Domain.Constants;
using Resource;

namespace Auth.Api.Application.Users.Commands.LoginUser;

public record LoginUserCommand : IRequest<LoginUserResponse>
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
}

public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, LoginUserResponse>
{
    private readonly IJwtService _jwtService;
    private readonly ISignInService _signInService;
    private readonly IUserManagerService _userManagerService;

    public LoginUserCommandHandler(IUserManagerService userManagerService, ISignInService signInService,
        IJwtService jwtService)
    {
        _userManagerService = userManagerService;
        _signInService = signInService;
        _jwtService = jwtService;
    }

    public async Task<LoginUserResponse> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManagerService.GetUserByEmailAsync(request.Email);

        if (user is null)
        {
            throw new UnauthorizedAccessException();
        }

        var isAdmin = await _userManagerService.IsInRoleAsync(user.Id, Roles.Administrator);

        var result = await _signInService.CheckPasswordSignInAsync(user, request.Password, !isAdmin);

        if (result.IsLockedOut)
        {
            throw new UnauthorizedAccessException(UserErrorMessages.ToManyAttempts);
        }

        if (!result.Succeeded)
        {
            throw new UnauthorizedAccessException();
        }

        var roles = await _userManagerService.GetUserRolesAsync(user);

        var token = _jwtService.GenerateJwtToken(user, roles.ToList());

        return new LoginUserResponse { Token = token };
    }
}
