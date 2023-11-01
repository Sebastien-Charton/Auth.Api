using Auth.Api.Application.Environment.Queries;

namespace Auth.Api.Web.Endpoints;

public class EnvironmentEndpoints : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .MapGet(GetEnvironment);
    }

    public async Task<GetEnvironmentDto> GetEnvironment(ISender sender)
    {
        return await sender.Send(new GetEnvironmentQuery());
    }
}
