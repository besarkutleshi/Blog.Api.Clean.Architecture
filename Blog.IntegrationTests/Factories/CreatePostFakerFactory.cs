using Blog.Application.Features.Posts.Dtos.Requests;
using Blog.IntegrationTests.Helpers;

namespace Blog.IntegrationTests.Factories;

public static class CreatePostFakerFactory
{
    public static CreatePostDto Create()
    {
        var title = RandomStringGenerator.GenerateRandomString(10, 90);
        var content = RandomStringGenerator.GenerateRandomString(10, 90);
        var friendlyUrl = RandomStringGenerator.GenerateRandomString(10, 90);

        return new CreatePostDto(content, title, friendlyUrl);
    }
}
