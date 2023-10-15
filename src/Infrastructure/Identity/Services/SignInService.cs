using Auth.Api.Application.Common.Interfaces.Identity.Models;
using Auth.Api.Application.Common.Interfaces.Identity.Services;
using Auth.Api.Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Identity;

namespace Auth.Api.Infrastructure.Identity.Services;

public class SignInService : ISignInService
{
    private readonly SignInManager<ApplicationUser> _signInManager;

    public SignInService(SignInManager<ApplicationUser> signInManager)
    {
        _signInManager = signInManager;
    }

    public async Task<ICheckPasswordSignInResponse> CheckPasswordSignInAsync(IApplicationUser user, string password)
    {
        // TODO lockout to false
        var result = await _signInManager.CheckPasswordSignInAsync((ApplicationUser)user, password, false);
        return new CheckPasswordSignInResponse(result.Succeeded, result.IsLockedOut, result.IsNotAllowed,
            result.RequiresTwoFactor);
    }
}
