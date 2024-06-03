using Blog.Domain.Entities;
using Blog.Domain.EntityErrors;
using Blog.Infrastructure.Persistence;
using Blog.Infrastructure.Repositories;
using Blog.IntegrationTests.Factories;
using Blog.SharedResources;
using Bogus;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace Blog.IntegrationTests.Infrastructure.Repositories;

public class PostRepositoryTests : BaseIntegrationTest
{
    private readonly ApplicationDbContext _dbContext;
    private readonly Faker<Post> _postFaker;
    private readonly PostRepository _postRepository;

    public PostRepositoryTests(CustomWebApplicationFactory factory)
        : base(factory)
    {
        _dbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        _postRepository = new PostRepository(_dbContext);
        _postFaker = PostFakerFactory.Create();
    }

    [Fact]
    public async Task Should_Have_No_Error_When_Creating_New_Post_With_Unique_FriendlyUrl()
    {
        var user = await CreateUser();

        var post = _postFaker.Generate();
        post.CreatedBy = user.Id;

        var result = await _postRepository.CreatePost(post, It.IsAny<CancellationToken>());

        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Response);
        Assert.NotNull(result.Response.Result);
        Assert.IsType<Post>(result.Response.Result);
        Assert.Equal(SuccessType.Created, result.Response.Type);
    }

    [Fact]
    public async Task Should_Have_Error_When_FrienldyUrl_Is_Not_Unique()
    {
        var user = await CreateUser();
        var createdPosts = await CreatePost(user.Id);

        var result = await _postRepository.CreatePost(createdPosts[0], It.IsAny<CancellationToken>());
        
        Assert.NotNull(result);
        Assert.True(result.IsFailure);
        Assert.NotNull(result.Error);
        Assert.Single(result.Error.ErrorMessages);
        Assert.Equal(PostErrors.PostWithSameFriendlyUrlExists(createdPosts[0].FriendlyUrl!).ErrorMessages[0], result.Error.ErrorMessages[0]);
    }

    [Fact]
    public async Task Should_Have_Error_When_Post_Not_Found_To_Delete()
    {
        var postId = new Random().Next(80000, 100000);

        var result = await _postRepository.DeletePost(postId, It.IsAny<CancellationToken>());

        Assert.NotNull(result);
        Assert.True(result.IsFailure);
        Assert.NotNull(result.Error);
        Assert.Single(result.Error.ErrorMessages);
        Assert.Equal(PostErrors.PostNotFound(postId).ErrorMessages[0], result.Error.ErrorMessages[0]);
    }

    [Fact]
    public async Task Should_Have_No_Error_When_Deleting_An_Existing_Post()
    {
        var user = await CreateUser();
        var createdPosts = await CreatePost(user.Id);

        var result = await _postRepository.DeletePost(createdPosts[0].Id, It.IsAny<CancellationToken>());

        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Response);
        Assert.Equal(SuccessType.NoContent, result.Response.Type);
    }

    [Fact]
    public async Task Should_Have_No_Error_When_Getting_Posts()
    {
        var user = await CreateUser();
        await CreatePost(user.Id, 3);

        var result = await _postRepository.GetPosts(1, 10, It.IsAny<CancellationToken>());

        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Response);
        Assert.IsType<List<Post>>(result.Response.Result);
    }

    [Fact]
    public async Task Should_Have_Error_When_Post_Not_Found_To_Update()
    {
        var post = _postFaker.Generate();
        post.Id = 10;

        var result = await _postRepository.UpdatePost(post, It.IsAny<CancellationToken>());

        Assert.NotNull(result);
        Assert.True(result.IsFailure);
        Assert.NotNull(result.Error);
        Assert.Single(result.Error.ErrorMessages);
        Assert.Equal(PostErrors.PostNotFound(post.Id).ErrorMessages[0], result.Error.ErrorMessages[0]);
    }

    [Fact]
    public async Task Should_Have_No_Error_When_Updating_Post()
    {
        var user = await CreateUser();
        var createdPosts = await CreatePost(user.Id, 1);
        var post = createdPosts.First();

        post.Title = "Updated";

        var result = await _postRepository.UpdatePost(post, It.IsAny<CancellationToken>());

        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Response);
        Assert.NotNull(result.Response.Result);
        Assert.IsType<Post>(result.Response.Result);
        var updatedPost = (Post)result.Response.Result;
        Assert.Equal(post.Title, updatedPost.Title);
    }

    [Fact]
    public async Task Should_Have_Error_When_There_Is_No_Post_For_Importing()
    {
        List<Post> posts = null!;

        var result = await _postRepository.ImportPosts(posts, It.IsAny<CancellationToken>());

        Assert.NotNull(result);
        Assert.True(result.IsFailure);
        Assert.NotNull(result.Error);
        Assert.Single(result.Error.ErrorMessages);
        Assert.Equal(PostErrors.NoPostFoundToImport().ErrorMessages[0], result.Error.ErrorMessages[0]);
    }

    [Fact]
    public async Task Should_Have_Error_When_There_Is_No_Post_With_Unique_Friendly_Url()
    {
        var user = await CreateUser();
        var createdPosts = await CreatePost(user.Id, 3);

        var result = await _postRepository.ImportPosts(createdPosts, It.IsAny<CancellationToken>());

        Assert.NotNull(result);
        Assert.True(result.IsFailure);
        Assert.NotNull(result.Error);
        Assert.Single(result.Error.ErrorMessages);
        Assert.Equal(PostErrors.NoPostWithUniqueFriendlyUrl().ErrorMessages[0], result.Error.ErrorMessages[0]);
    }

    [Fact]
    public async Task Should_Have_No_Error_When_Importing_Posts_With_Unique_FriendlyUrl()
    {
        var user = await CreateUser();

        var posts = _postFaker.Generate(3);
        posts.ForEach(x => x.CreatedBy = user.Id);

        var result = await _postRepository.ImportPosts(posts, It.IsAny<CancellationToken>());

        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Response);
        Assert.NotNull(result.Response.Result);
        Assert.IsType<List<Post>>(result.Response.Result);
    }

    private async Task<List<Post>> CreatePost(string userId, int generateNumber = 1)
    {
        var posts = _postFaker.Generate(generateNumber);
        posts.ForEach(x => x.CreatedBy = userId);

        await _dbContext.Posts.AddRangeAsync(posts);

        await _dbContext.SaveChangesAsync();

        return posts;
    }
}
