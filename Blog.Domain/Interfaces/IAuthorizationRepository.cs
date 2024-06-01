using Blog.SharedResources;

namespace Blog.Domain.Interfaces;

public interface IAuthorizationRepository
{
    Task<Result> GetUserRoles(string userId, CancellationToken cancellationToken);
}
