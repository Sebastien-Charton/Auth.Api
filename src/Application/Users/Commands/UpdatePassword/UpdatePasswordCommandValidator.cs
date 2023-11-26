using Auth.Api.Application.Common.Extensions;

namespace Auth.Api.Application.Users.Commands.UpdatePassword;

public class UpdatePasswordCommandValidator : AbstractValidator<UpdatePasswordCommand>
{
    public UpdatePasswordCommandValidator()
    {
        RuleFor(x => x.CurrentPassword)
            .ShouldBeAPassword();

        RuleFor(x => x.NewPassword)
            .ShouldBeAPassword();
    }
}
