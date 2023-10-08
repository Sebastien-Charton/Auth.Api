using Auth.Api.Application.Common.Interfaces;

namespace Auth.Api.Application.Users.Commands.RegisterUser;

public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    private readonly IApplicationDbContext _context;

    public RegisterUserCommandValidator(IApplicationDbContext context)
    {
        _context = context;

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
            .MaximumLength(20)
            .Matches(@"\^[a-zA-Z0-9_.-]*$").WithMessage("UserName must contain only letters, number and - _ .");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty();
    }
}
