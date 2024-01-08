using Auth.Api.Application.Common.Interfaces;
using Auth.Api.Application.Common.Interfaces.Identity.Services;
using Auth.Api.Application.Common.Interfaces.Services;
using Auth.Api.Application.Common.Options;
using Auth.Api.Domain.Constants;
using Auth.Api.Domain.Entities;
using Microsoft.Extensions.Options;
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
    private readonly IApplicationDbContext _applicationDbContext;
    private readonly IOptions<RefreshTokenOptions> _refreshTokenOptions;

    public LoginUserCommandHandler(IUserManagerService userManagerService, ISignInService signInService,
        IJwtService jwtService, IApplicationDbContext applicationDbContext, IOptions<RefreshTokenOptions> refreshTokenOptions)
    {
        _userManagerService = userManagerService;
        _signInService = signInService;
        _jwtService = jwtService;
        _applicationDbContext = applicationDbContext;
        _refreshTokenOptions = refreshTokenOptions;
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

        string? refreshToken = null;
        
        var existingRefreshToken = await _applicationDbContext.RefreshTokens
            .FirstOrDefaultAsync(x => x.Revoked == null && x.UserId == user.Id &&  x.Expires > DateTimeOffset.UtcNow, cancellationToken);

        if(existingRefreshToken is null)
        {
            var generatedRefreshToken = _jwtService.GenerateRefreshToken();

            var refreshTokenEntity = new RefreshToken(user.Id, generatedRefreshToken, DateTimeOffset.Now.AddDays(_refreshTokenOptions.Value.ExpiryInDays));

            await _applicationDbContext.RefreshTokens.AddAsync(refreshTokenEntity, cancellationToken);

            await _applicationDbContext.SaveChangesAsync(cancellationToken);
            
            refreshToken = generatedRefreshToken;
        }
        else
        {
            refreshToken = existingRefreshToken.Token;
        }
        
        return new LoginUserResponse { Token = token, UserId = user.Id, RefreshToken = refreshToken};
    }
}
