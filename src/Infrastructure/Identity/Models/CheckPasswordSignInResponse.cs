using Auth.Api.Application.Common.Interfaces.Identity.Models;

namespace Auth.Api.Infrastructure.Identity.Models;

public class CheckPasswordSignInResponse : ICheckPasswordSignInResponse
{
    public CheckPasswordSignInResponse(bool succeeded, bool isLockedOut, bool isNotAllowed, bool requiresTwoFactor)
    {
        Succeeded = succeeded;
        IsLockedOut = isLockedOut;
        IsNotAllowed = isNotAllowed;
        RequiresTwoFactor = requiresTwoFactor;
    }

    public bool Succeeded { get; }

    public bool IsLockedOut { get; }

    public bool IsNotAllowed { get; }

    public bool RequiresTwoFactor { get; }
}
