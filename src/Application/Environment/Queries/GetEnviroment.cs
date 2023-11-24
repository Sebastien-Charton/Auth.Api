namespace Auth.Api.Application.Environment.Queries;

public record GetEnvironmentQuery : IRequest<GetEnvironmentDto>
{
}

public class GetEnvironmentQueryHandler : IRequestHandler<GetEnvironmentQuery, GetEnvironmentDto>
{
    public async Task<GetEnvironmentDto> Handle(GetEnvironmentQuery query, CancellationToken cancellationToken)
    {
        await Task.Delay(1);
        var environment = System.Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")!;
        return new GetEnvironmentDto(environment);
    }
}
