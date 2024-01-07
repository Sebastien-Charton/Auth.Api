namespace Auth.Api.Application.Users.Queries.IsUserNameExists;

public class IsUserNameExistsQueryValidator : AbstractValidator<IsUserNameExistsQuery>
{
    public IsUserNameExistsQueryValidator()
    {
        RuleFor(x => x.UserName)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(20)
            .Matches(@"^[\w.-]+$").WithMessage("UserName must contain only letters or number or . -");
    }
}
