using Blog.Application.Contracts;
using Blog.Application.Factories;
using Blog.Application.Features.Auth.Dtos.Responses;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Moq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Xunit;
using static System.Collections.Specialized.BitVector32;

namespace Blog.UnitTests.Applications.Factories;

public class AuthTokenFactoryTests
{
    private readonly Mock<IConfiguration> _configurationMock;
    private readonly AuthTokenFactory _authTokenFactory;
    private readonly string _validSecurityKey = "RhirgRldvmEjHtBcHVQ4huy4VJGtTLXd90KysYVJEpd0Z3xGXqYoYn1ZsGq8iWSfm1IFzL861WwGFWZO7LSIuribCmTqsSZNBFqlhrngE7xtawasdwss";

    private IConfigurationRoot GetConfiguration(string securityKey)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string>
            {
                {"tokenSecurityKey", $"{securityKey}"},
            })
            .Build();

        return configuration;
    }

    public AuthTokenFactoryTests()
    {
        var configuration = GetConfiguration(_validSecurityKey);
        IConfigurationSection section = configuration.GetSection("tokenSecurityKey");
        _configurationMock = new Mock<IConfiguration>();
        _configurationMock.Setup(x => x.GetSection("tokenSecurityKey")).Returns(section);
        _authTokenFactory = new AuthTokenFactory(_configurationMock.Object);
    }

    [Fact]
    public void GenerateToken_Should_ThrowException_When_SecurityKey_IsNullOrEmpty()
    {
        var configuration = GetConfiguration("");
        IConfigurationSection section = configuration.GetSection("tokenSecurityKey");

        var invalidConfigurationMock = new Mock<IConfiguration>();
        invalidConfigurationMock.Setup(x => x.GetSection("tokenSecurityKey")).Returns(section);

        var authTokenFactory = new AuthTokenFactory(invalidConfigurationMock.Object);
        Action act = () => authTokenFactory.GenerateToken(new UserInfoDto("id", "email", "", ["Admin"]));

        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void GenerateToken_Should_ReturnNull_When_UserInfo_IsNull()
    {
        Action act = () => _authTokenFactory.GenerateToken(null!);

        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void GenerateToken_Should_ReturnNull_When_UserInfo_Id_IsEmpty()
    {
        Action act = () => _authTokenFactory.GenerateToken(new UserInfoDto(string.Empty, "Valid email", "", ["Admin"]));

        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void GenerateToken_Should_ReturnNull_When_UserInfo_Email_IsEmpty()
    {
        Action act = () => _authTokenFactory.GenerateToken(new UserInfoDto("Valid id", string.Empty, "", ["Admin"]));

        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void GenerateToken_Should_ReturnNull_When_UserInfo_Roles_IsNull()
    {
        Action act = () => _authTokenFactory.GenerateToken(new UserInfoDto("Valid id", "Valid email", "", null!));

        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void GenerateToken_Should_ReturnNull_When_UserInfo_Roles_IsEmpty()
    {
        Action act = () => _authTokenFactory.GenerateToken(new UserInfoDto("Valid id", "Valid email", "", []));

        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void GenerateToken_Should_ReturnValidToken_When_UserInfo_IsValid()
    {
        var validUserInfo = new UserInfoDto("1", "test@example.com", "", ["Admin"]);

        var token = _authTokenFactory.GenerateToken(validUserInfo);

        token.Should().NotBeNullOrEmpty();
        ValidateToken(token!, validUserInfo);
    }

    private void ValidateToken(string token, UserInfoDto userInfo)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_validSecurityKey);

        tokenHandler.ValidateToken(token, new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false,
            ClockSkew = TimeSpan.Zero
        }, out SecurityToken validatedToken);

        var jwtToken = (JwtSecurityToken)validatedToken;

        var email = jwtToken.Claims.FirstOrDefault(x => x.Type == "email")!.Value;
        var id = jwtToken.Claims.FirstOrDefault(x => x.Type == "unique_name")!.Value;
        var role = jwtToken.Claims.FirstOrDefault(x => x.Type == "role")!.Value;

        Assert.Equal(userInfo.Email, email);
        Assert.Equal(userInfo.Id, id);
        Assert.Equal(string.Join(',', userInfo.Roles), role);
    }
}
