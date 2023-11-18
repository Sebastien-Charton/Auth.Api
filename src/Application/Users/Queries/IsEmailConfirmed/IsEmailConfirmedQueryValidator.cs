namespace Auth.Api.Application.Users.Queries.IsEmailConfirmed;

public class IsEmailConfirmedQueryValidator : AbstractValidator<IsEmailConfirmedQuery>
{
    public IsEmailConfirmedQueryValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty();
    }
}
