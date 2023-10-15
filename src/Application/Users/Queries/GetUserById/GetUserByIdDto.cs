using Auth.Api.Application.Common.Interfaces.Identity.Models;

namespace Auth.Api.Application.Users.Queries.GetUserById;

public class GetUserByIdDto
{
    public Guid Id { get; init; }

    public string Email { get; init; } = null!;
    public string UserName { get; set; } = null!;

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<IApplicationUser, GetUserByIdDto>();
        }
    }
}
