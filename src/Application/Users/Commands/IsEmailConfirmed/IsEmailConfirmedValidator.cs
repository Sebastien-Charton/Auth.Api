namespace Auth.Api.Application.Users.Commands.IsEmailConfirmed;

public class IsEmailConfirmedValidator : AbstractValidator<IsEmailConfirmedCommand>
{
    public IsEmailConfirmedValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty();
    }
}
