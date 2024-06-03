namespace Blog.Infrastructure.Persistence;

public class DbConnectionStringOptions
{
    public string Server { get; set; } = null!;
    public string Database { get; set; } = null!;
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string IntegrationTestDatabase { get; set; } = null!;
}
