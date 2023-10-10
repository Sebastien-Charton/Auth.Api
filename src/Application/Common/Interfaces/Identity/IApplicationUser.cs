namespace Auth.Api.Application.Common.Interfaces.Identity;

public interface IApplicationUser
{
    public Guid Id { get; set; }
    public string? Email { get; set; }
    public string? UserName { get; set; }
}
