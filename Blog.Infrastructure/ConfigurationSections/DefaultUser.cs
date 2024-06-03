namespace Blog.Infrastructure.ConfigurationSections;

public class DefaultUser
{
    public string Id { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public string Username { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public bool EmailConfirmed { get; set; }
}
