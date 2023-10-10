using Auth.Api.Application.Common.Interfaces.Identity;
using Microsoft.AspNetCore.Identity;

namespace Auth.Api.Infrastructure.Identity;

public class ApplicationUser : IdentityUser<Guid>, IApplicationUser
{
}
