﻿using Auth.Api.Application.Common.Models;

namespace Auth.Api.Application.Common.Interfaces;

public interface IIdentityService
{
    Task<string?> GetUserNameAsync(Guid userId);

    Task<bool> IsInRoleAsync(Guid userId, string role);

    Task<bool> AuthorizeAsync(Guid userId, string policyName);

    Task<(Result Result, Guid userId)> CreateUserAsync(string userName, string password, string email,
        string phoneNumber);

    Task<Result> DeleteUserAsync(Guid userId);
    Task<string?> GetUserByUserNameAsync(string userName);
    Task<string?> GetUserByEmailAsync(string email);
}
