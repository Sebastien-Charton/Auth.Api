namespace Auth.Api.Domain.Entities;

public class RefreshToken : BaseAuditableEntity
{
    private DateTimeOffset _expires;
    public Guid UserId { get; set; }
    public string Token { get; set; }

    public DateTimeOffset Expires
    {
        get => _expires;
    }

    public void SetExpires(DateTimeOffset value) => _expires = value.ToUniversalTime();

    public DateTime? Revoked { get; set; }
    public Guid? RevokedBy { get; set; }
    
    public RefreshToken(Guid userId, string token, DateTimeOffset expires)
    {
        UserId = userId;
        Token = token;
        SetExpires(expires);
        Revoked = null;
        RevokedBy = null;
    }
}
