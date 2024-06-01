using Blog.SharedResources;

namespace Blog.Domain.Interfaces;

public interface IAuthenticationRepository
{
    Task<Result> Login(string email, string password, CancellationToken cancellationToken);
}
