using Auth.Api.Application.Common.Interfaces.Identity.Models;

namespace Auth.Api.Application.Users.Queries.GetUserById;

public class GetUserByIdResponse
{
    public GetUserByIdResponse(Guid id, string email, string userName)
    {
        Id = id;
        Email = email;
        UserName = userName;
    }

    public Guid Id { get; init; }

    public string Email { get; init; }
    public string UserName { get; set; }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<IApplicationUser, GetUserByIdResponse>();
        }
    }
}
