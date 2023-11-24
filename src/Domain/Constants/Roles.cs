using Auth.Api.Domain.Extensions;

namespace Auth.Api.Domain.Constants;

public abstract class Roles
{
    public const string Administrator = nameof(Administrator);
    public const string User = nameof(User);
    public static string[] GetRoles => typeof(Roles).GetAllPublicConstantValues<string>();
}
