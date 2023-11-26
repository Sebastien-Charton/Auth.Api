using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Auth.Api.Application.Common.Interfaces.Identity.Models;
using Auth.Api.Application.Common.Interfaces.Services;
using Auth.Api.Infrastructure.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Auth.Api.Infrastructure.Services;

public class JwtService : IJwtService
{
    private readonly IOptions<JwtOptions> _jwtOptions;

    public JwtService(IOptions<JwtOptions> jwtOptions)
    {
        _jwtOptions = jwtOptions;
    }

    public string GenerateJwtToken(IApplicationUser user, List<string>? roles)
    {
        var claims = new List<Claim> { new(ClaimTypes.NameIdentifier, user.Id.ToString()) };

        if (user.Email is not null)
        {
            claims.Add(new Claim(ClaimTypes.Email, user.Email));
        }

        if (user.UserName is not null)
        {
            claims.Add(new Claim(ClaimTypes.Surname, user.UserName));
        }

        foreach (var role in roles ?? Enumerable.Empty<string>())
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Value.SecurityKey));

        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
        var jwtExpiry = DateTime.Now.AddDays(_jwtOptions.Value.ExpiryInDays);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Expires = jwtExpiry,
            SigningCredentials = credentials,
            Subject = new ClaimsIdentity(claims),
            Audience = _jwtOptions.Value.Audience,
            Issuer = _jwtOptions.Value.Issuer
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
