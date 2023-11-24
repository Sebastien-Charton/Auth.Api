using Auth.Api.Domain.Constants;

namespace Auth.Api.Application.Users.Commands.RegisterUserAdmin;

public class RegisterUserAdminCommandValidator : AbstractValidator<RegisterUserAdminCommand>
{
    public RegisterUserAdminCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(8)
            .Matches(@"\d").WithMessage("Password must contain at least one digit.")
            .Matches(@"\W").WithMessage("Password must contain at least one non-alphanumeric character.")
            .Matches(@"[A-Z]").WithMessage("Password must contain at least one uppercase letter.");

        RuleFor(x => x.UserName)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(64)
            .Matches(@"^[\w.-]+$").WithMessage("UserName must contain only letters or number or . -");

        RuleFor(x => x.Roles)
            .NotEmpty()
            .Must(x => x.All(y => Roles.GetRoles.Contains(y)));
    }
}
