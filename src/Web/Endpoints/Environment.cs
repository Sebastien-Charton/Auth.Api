using Auth.Api.Application.Environment.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Api.Web.Endpoints;

public class Environment : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .MapGet(GetEnvironment);
    }

    [ProducesResponseType(typeof(GetEnvironmentDto), 200)]
    public async Task<GetEnvironmentDto> GetEnvironment(ISender sender)
    {
        return await sender.Send(new GetEnvironmentQuery());
    }
}
