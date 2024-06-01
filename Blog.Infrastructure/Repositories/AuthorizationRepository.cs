using Blog.Domain.EntityErrors;
using Blog.Domain.Interfaces;
using Blog.SharedResources;
using Microsoft.AspNetCore.Identity;

namespace Blog.Infrastructure.Repositories;

public class AuthorizationRepository(UserManager<IdentityUser> _userManager) : IAuthorizationRepository
{
    public async Task<Result> GetUserRoles(string userId, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null)
            return Result.Failure(UserErrors.UserNotFoundById(userId));

        var roles = await _userManager.GetRolesAsync(user);
        if (roles == null || roles.Count < 1)
            return Result.Failure(UserErrors.NoRoleAssigned());

        return Result.Success(Success.Ok(roles));
    }
}
