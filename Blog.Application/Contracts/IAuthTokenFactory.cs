using Blog.Application.Features.Auth.Dtos.Responses;

namespace Blog.Application.Contracts;

public interface IAuthTokenFactory
{
    string? GenerateToken(UserInfoDto userInfo);
}
