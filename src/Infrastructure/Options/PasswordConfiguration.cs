namespace Auth.Api.Infrastructure.Options;

public class PasswordConfiguration
{
    public int SaltSize { get; set; }
    public int Iterations { get; set; }
    public int HashSize { get; set; }
}
