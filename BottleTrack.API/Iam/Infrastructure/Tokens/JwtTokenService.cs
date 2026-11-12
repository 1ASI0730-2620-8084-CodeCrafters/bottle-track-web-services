using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BottleTrack.API.Iam.Application.Services;
using BottleTrack.API.Iam.Domain.Model.Aggregates;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace BottleTrack.API.Iam.Infrastructure.Tokens;

public class JwtTokenService : ITokenService
{
    private readonly JwtSettings settings;

    public JwtTokenService(IOptions<JwtSettings> options)
    {
        settings = options.Value;
    }

    public string Generate(User user)
    {
        JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
        byte[] key = Encoding.UTF8.GetBytes(settings.Secret);

        List<Claim> claims = new List<Claim>();
        claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()));
        claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email.Value));
        claims.Add(new Claim(ClaimTypes.Role, user.Role.ToString()));
        claims.Add(new Claim("organization_id", user.OrganizationId.ToString()));

        SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor();
        descriptor.Subject = new ClaimsIdentity(claims);
        descriptor.Expires = DateTime.UtcNow.AddDays(settings.ExpiresInDays);
        descriptor.Issuer = settings.Issuer;
        descriptor.Audience = settings.Audience;
        descriptor.SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
            SecurityAlgorithms.HmacSha256Signature);

        SecurityToken token = tokenHandler.CreateToken(descriptor);
        return tokenHandler.WriteToken(token);
    }
}
