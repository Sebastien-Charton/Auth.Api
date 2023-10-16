using Auth.Api.Application.Common.Interfaces.Identity.Services;
using Auth.Api.Application.Common.Interfaces.Services;

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
        var existingUserWithEmail = await _userManagerService.GetUserByEmailAsync(request.Email);

        if (existingUserWithEmail is null)
        {
            throw new UnauthorizedAccessException();
        }

        var result = await _signInService.CheckPasswordSignInAsync(existingUserWithEmail, request.Password);

        if (!result.Succeeded)
        {
            throw new UnauthorizedAccessException();
        }

        var roles = await _userManagerService.GetUserRolesAsync(existingUserWithEmail);

        var token = _jwtService.GenerateJwtToken(existingUserWithEmail, roles.ToList());

        return new LoginUserResponse { Token = token };
    }
}
