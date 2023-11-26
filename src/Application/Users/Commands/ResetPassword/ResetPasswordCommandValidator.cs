using Auth.Api.Application.Common.Extensions;

namespace Auth.Api.Application.Users.Commands.ResetPassword;

public class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordCommand>
{
    public ResetPasswordCommandValidator()
    {
        RuleFor(x => x.NewPassword)
            .ShouldBeAPassword();

        RuleFor(x => x.Token)
            .NotEmpty();
    }
}
