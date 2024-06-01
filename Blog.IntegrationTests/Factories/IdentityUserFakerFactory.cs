using Bogus;
using Microsoft.AspNetCore.Identity;

namespace Blog.IntegrationTests.Factories;

public static class IdentityUserFakerFactory
{
    public static Faker<IdentityUser> Create()
    {
        return new Faker<IdentityUser>()
           .RuleFor(x => x.Id, Guid.NewGuid().ToString())
           .RuleFor(x => x.UserName, f => f.Internet.UserName())
           .RuleFor(x => x.Email, f => f.Internet.Email())
           .RuleFor(x => x.PhoneNumber, f => f.Phone.PhoneNumber())
           .RuleFor(x => x.EmailConfirmed, true);
    }
}
