namespace Auth.Api.Application.Environment.Queries;

public class GetEnvironmentDto
{
    public GetEnvironmentDto(string environment)
    {
        Environment = environment;
    }

    public string Environment { get; set; }
}
