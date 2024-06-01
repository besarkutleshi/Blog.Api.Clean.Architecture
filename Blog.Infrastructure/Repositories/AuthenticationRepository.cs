using Blog.Domain.EntityErrors;
using Blog.Domain.Interfaces;
using Blog.SharedResources;
using Microsoft.AspNetCore.Identity;

namespace Blog.Infrastructure.Repositories;

public class AuthenticationRepository(UserManager<IdentityUser> _userManager, 
    SignInManager<IdentityUser> _signInManager) : IAuthenticationRepository
{
    public async Task<Result> Login(string email, string password, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
        {
            return Result.Failure(UserErrors.UserNotFound(email));
        }

        var result = await _signInManager.PasswordSignInAsync(user, password, true, true);
        if (result.Succeeded)
        {
            return Result.Success(Success.Ok(user));
        }

        return Result.Failure(UserErrors.IncorrectPasswrod());
    }
}
