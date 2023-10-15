namespace Auth.Api.Application.Common.Interfaces.Identity.Models;

public interface ICheckPasswordSignInResponse
{
    public bool Succeeded { get; }

    public bool IsLockedOut { get; }

    public bool IsNotAllowed { get; }

    public bool RequiresTwoFactor { get; }
}
