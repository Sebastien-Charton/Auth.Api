using Auth.Api.Application.Common.Interfaces.Identity.Services;
using Auth.Api.Domain.Constants;
using FluentValidation.Results;
using ValidationException = Auth.Api.Application.Common.Exceptions.ValidationException;

namespace Auth.Api.Application.Users.Commands.RegisterUser;

public record RegisterUserCommand : IRequest<Guid>
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string UserName { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
}

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Guid>
{
    private readonly IUserManagerService _userManagerService;

    public RegisterUserCommandHandler(IUserManagerService userManagerService)
    {
        _userManagerService = userManagerService;
    }

    public async Task<Guid> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        string? existingUserWithUserName = await _userManagerService.GetUserByUserNameAsync(request.UserName);

        var validationExceptions = new List<ValidationFailure>();
        if (existingUserWithUserName is not null)
        {
            // TODO error message magic string
            var validationException = new ValidationFailure(nameof(request.UserName), "UserName is already used.");
            validationExceptions.Add(validationException);
        }

        var existingUserWithEmail = await _userManagerService.GetUserByEmailAsync(request.Email);

        if (existingUserWithEmail is not null)
        {
            // TODO error message magic string
            var validationException = new ValidationFailure(nameof(request.Email), "Email is already used.");
            validationExceptions.Add(validationException);
        }

        if (validationExceptions.Any())
        {
            throw new ValidationException(validationExceptions);
        }

        (Common.Models.Result Result, Guid userId) result = await _userManagerService.CreateUserAsync(request.UserName,
            request.Password,
            request.Email, request.PhoneNumber);

        if (result.Result.Errors.Any())
        {
            throw new ValidationException(
                result.Result.Errors.Select(x => new ValidationFailure(nameof(RegisterUserCommand), x)));
        }

        await _userManagerService.AddToRolesAsync(result.userId, new[] { Roles.User });

        return result.userId;
    }
}
