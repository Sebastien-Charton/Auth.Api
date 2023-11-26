namespace Auth.Api.Application.Common.Extensions;

public static class PasswordValidator
{
    public static IRuleBuilderOptions<T, string> ShouldBeAPassword<T>(
        this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .MinimumLength(8)
            .Matches(@"\d").WithMessage("Password must contain at least one digit.")
            .Matches(@"\W").WithMessage("Password must contain at least one non-alphanumeric character.")
            .Matches(@"[A-Z]").WithMessage("Password must contain at least one uppercase letter.");
    }
}
