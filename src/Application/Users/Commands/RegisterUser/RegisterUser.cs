using Auth.Api.Application.Common.Interfaces;
using FluentValidation.Results;
using ValidationException = System.ComponentModel.DataAnnotations.ValidationException;

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
    private readonly IIdentityService _identityService;

    public RegisterUserCommandHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task<Guid> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        string? existingUserWithUserName = await _identityService.GetUserByUserNameAsync(request.UserName);

        if (existingUserWithUserName is not null)
        {
            throw new ValidationException("UserName is already used.");
        }

        string? existingUserWithEmail = await _identityService.GetUserByEmailAsync(request.Email);

        if (existingUserWithEmail is not null)
        {
            throw new ValidationException("Email is already used.");
        }

        (Common.Models.Result Result, Guid userId) result = await _identityService.CreateUserAsync(request.UserName,
            request.Password,
            request.Email, request.PhoneNumber);

        if (result.Result.Errors.Any())
        {
            throw new Common.Exceptions.ValidationException(
                result.Result.Errors.Select(x => new ValidationFailure(nameof(RegisterUserCommand), x)));
        }

        return result.userId;
    }
}
