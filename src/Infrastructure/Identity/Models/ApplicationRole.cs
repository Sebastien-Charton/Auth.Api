using Auth.Api.Application.Common.Interfaces.Identity.Models;
using Microsoft.AspNetCore.Identity;

namespace Auth.Api.Infrastructure.Identity.Models;

public class ApplicationRole : IdentityRole<Guid>, IApplicationRole
{
    public ApplicationRole()
    {
    }

    public ApplicationRole(string roleName) : base(roleName)
    {
    }
}
