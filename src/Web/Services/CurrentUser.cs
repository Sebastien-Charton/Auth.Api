using System.Security.Claims;
using Auth.Api.Application.Common.Interfaces;

namespace Auth.Api.Web.Services;

public class CurrentUser : IUser
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUser(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid GetId()
    {
        var isGuidParsed = Guid.TryParse(
            _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier),
            out Guid result);

        if (!isGuidParsed || result == Guid.Empty)
        {
            throw new UnauthorizedAccessException();
        }

        return result;
    }
}
