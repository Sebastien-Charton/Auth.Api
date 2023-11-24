using Auth.Api.Application.Environment.Queries;
using Auth.Api.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Api.Web.Endpoints;

[Authorize(Policy = Policies.AllUsers)]
public class Environment : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .MapGet(GetEnvironment);
    }

    [ProducesResponseType(typeof(GetEnvironmentDto), 200)]
    [EndpointDescription("Get the current environment")]
    public async Task<GetEnvironmentDto> GetEnvironment(ISender sender)
    {
        return await sender.Send(new GetEnvironmentQuery());
    }
}
