using Auth.Api.Application.Common.Interfaces.Identity.Models;

namespace Auth.Api.Application.Common.Interfaces.Services;

public interface IJwtService
{
    public string GenerateJwtToken(IApplicationUser user, List<string>? roles);
    string GenerateRefreshToken();
}
