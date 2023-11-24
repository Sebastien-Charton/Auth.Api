namespace Auth.Api.Application.Users.Queries.IsEmailExists;

public class IsEmailExistsQueryValidator : AbstractValidator<IsEmailExistsQuery>
{
    public IsEmailExistsQueryValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();
    }
}
