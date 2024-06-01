using Blog.Application.Contracts;
using Blog.Application.Features.Auth.Dtos.Responses;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Blog.Application.Factories;

public class AuthTokenFactory(IConfiguration configuration) : IAuthTokenFactory
{
    private readonly string _securityKey = configuration.GetSection("tokenSecurityKey").Value!;

    public string? GenerateToken(UserInfoDto userInfo)
    {
        if (IsUserInfoValid(userInfo))
            throw new ArgumentNullException(nameof(userInfo));

        if (string.IsNullOrEmpty(_securityKey))
            throw new ArgumentNullException(nameof(configuration));

        var secretBytes = Encoding.ASCII.GetBytes(_securityKey);

        JwtSecurityTokenHandler handler = new();

        SymmetricSecurityKey key = new(secretBytes);

        SecurityTokenDescriptor descriptor = new()
        {
            Subject = GetClaims(userInfo),
            Expires = DateTime.Now.AddYears(20),
            SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature)
        };

        JwtSecurityToken token = handler.CreateJwtSecurityToken(descriptor);

        return handler.WriteToken(token);
    }

    private static ClaimsIdentity GetClaims(UserInfoDto userInfo)
    {
        return new ClaimsIdentity(
            getClaims(userInfo));

        static Claim[] getClaims(UserInfoDto userInfo)
        {
            List<Claim> claims =
            [
                new Claim(ClaimTypes.Email, userInfo.Email),
                new Claim(ClaimTypes.Name, userInfo.Id),
            ];

            if (userInfo.Roles != null && userInfo.Roles.Count > 0)
            {
                claims.AddRange(userInfo.Roles.Select(x => new Claim(ClaimTypes.Role, x)));
            }

            return [.. claims];
        }
    }

    private static bool IsUserInfoValid(UserInfoDto userInfo)
        => userInfo == null || string.IsNullOrEmpty(userInfo.Id) || string.IsNullOrEmpty(userInfo.Email) || userInfo.Roles == null || userInfo.Roles.Count < 1;
}
