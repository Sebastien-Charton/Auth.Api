﻿using Auth.Api.Application.Common.Interfaces.Identity.Services;

namespace Auth.Api.Application.Users.Commands.IsEmailConfirmed;

public record IsEmailConfirmedCommand : IRequest<bool>
{
    public Guid UserId { get; set; }
}

public class ValidateEmailCommandHandler : IRequestHandler<IsEmailConfirmedCommand, bool>
{
    private readonly IUserManagerService _userManagerService;

    public ValidateEmailCommandHandler(IUserManagerService userManagerService)
    {
        _userManagerService = userManagerService;
    }

    public async Task<bool> Handle(IsEmailConfirmedCommand request, CancellationToken cancellationToken)
    {
        return await _userManagerService.IsEmailConfirmed(request.UserId);
    }
}