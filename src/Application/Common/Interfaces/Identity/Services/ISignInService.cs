using Auth.Api.Application.Common.Interfaces.Identity.Models;

namespace Auth.Api.Application.Common.Interfaces.Identity.Services;

public interface ISignInService
{
    Task<ICheckPasswordSignInResponse> CheckPasswordSignInAsync(IApplicationUser user, string password);
}
