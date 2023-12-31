﻿using Auth.Api.Application.Common.Interfaces;
using Auth.Api.Application.Common.Interfaces.Identity.Services;
using Resource;

namespace Auth.Api.Application.Users.Commands.PasswordResetToken;

public record PasswordResetTokenCommand : IRequest<PasswordResetTokenResponse>
{
}

public class PasswordResetTokenCommandHandler : IRequestHandler<PasswordResetTokenCommand,
    PasswordResetTokenResponse>
{
    private readonly IUser _user;
    private readonly IUserManagerService _userManagerService;

    public PasswordResetTokenCommandHandler(IUserManagerService userManagerService, IUser user)
    {
        _userManagerService = userManagerService;
        _user = user;
    }

    public async Task<PasswordResetTokenResponse> Handle(PasswordResetTokenCommand request,
        CancellationToken cancellationToken)
    {
        var user = await _userManagerService.GetUserByIdAsync(_user.Id!.Value);
        var token = await _userManagerService.GenerateResetPasswordTokenAsync(user!);

        if (token is null)
        {
            throw new Exception(GeneralErrorMessages.UnkownError);
        }

        return new PasswordResetTokenResponse(token);
    }
}
