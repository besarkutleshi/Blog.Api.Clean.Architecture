using Blog.Domain.Entities;
using Bogus;
using Bogus.Extensions;

namespace Blog.IntegrationTests.Factories;

public static class PostFakerFactory
{
    public static Faker<Post> Create()
    {
        return new Faker<Post>()
            .RuleFor(x => x.Title, f => f.Lorem.Text().ClampLength(99, 99))
            .RuleFor(x => x.Content, f => f.Lorem.Text().ClampLength(999, 999))
            .RuleFor(x => x.FriendlyUrl, f => f.Lorem.Text().ClampLength(100, 150) + $"{Guid.NewGuid()}")
            .RuleFor(x => x.DateCreated, DateTime.UtcNow);
    }
}
