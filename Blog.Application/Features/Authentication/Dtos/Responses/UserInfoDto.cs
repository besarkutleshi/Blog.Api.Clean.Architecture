namespace Blog.Application.Features.Auth.Dtos.Responses;

public class UserInfoDto(string id, string email, string token, List<string> roles)
{
    public string Id { get; set; } = id;
    public string Email { get; set; } = email;
    public string Token { get; set; } = token;
    public List<string> Roles { get; set; } = roles;
}
