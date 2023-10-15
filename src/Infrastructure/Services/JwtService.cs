using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Auth.Api.Application.Common.Interfaces.Identity.Models;
using Auth.Api.Application.Common.Interfaces.Services;
using Auth.Api.Infrastructure.Settings;
using Microsoft.IdentityModel.Tokens;

namespace Auth.Api.Infrastructure.Services;

public class JwtService : IJwtService
{
    private readonly JwtConfiguration _jwtConfiguration;

    public JwtService(JwtConfiguration jwtConfiguration)
    {
        _jwtConfiguration = jwtConfiguration;
    }

    public string GenerateJwtToken(IApplicationUser user, List<string>? roles)
    {
        // TODO should I put the guards or put a default value in the claims below
        Guard.Against.Null(user.Email);
        Guard.Against.Null(user.UserName);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.Email),
            new(ClaimTypes.Surname, user.UserName)
        };

        foreach (var role in roles ?? Enumerable.Empty<string>())
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfiguration.SecurityKey));

        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
        var jwtExpiry = DateTime.Now.AddDays(_jwtConfiguration.ExpiryInDays);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Expires = jwtExpiry,
            SigningCredentials = credentials,
            Subject = new ClaimsIdentity(claims),
            Audience = _jwtConfiguration.Audience,
            Issuer = _jwtConfiguration.Issuer
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
