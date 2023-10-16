namespace Auth.Api.Infrastructure.Options;

public class IdentityOptions
{
    public required Password Password { get; set; }
    public required Lockout Lockout { get; set; }
    public required User User { get; set; }
    public required SignIn SignIn { get; set; }
}

public class Lockout
{
    public int DefaultLockoutTimeSpanInMs { get; set; }
    public int MaxFailedAccessAttempts { get; set; }
    public bool AllowedForNewUsers { get; set; }
    public bool LockoutOnFailure { get; set; }
}

public class Password
{
    public bool RequireDigit { get; set; }
    public bool RequireLowercase { get; set; }
    public bool RequireNonAlphanumeric { get; set; }
    public bool RequireUppercase { get; set; }
    public int RequiredLength { get; set; }
    public int RequiredUniqueChars { get; set; }
}

public class SignIn
{
    public bool RequireConfirmedPhoneNumber { get; set; }
    public bool RequireConfirmedAccount { get; set; }
    public bool RequireConfirmedEmail { get; set; }
}

public class User
{
    public bool RequireUniqueEmail { get; set; }
    public required string AllowedUserNameCharacters { get; set; }
}
