using Auth.Api.Application.Common.Extensions;

namespace Auth.Api.Application.Users.Commands.RegisterUser;

public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.Password)
            .ShouldBeAPassword();

        RuleFor(x => x.UserName)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(20)
            .Matches(@"^[\w.-]+$").WithMessage("UserName must contain only letters or number or . -");
    }
}
