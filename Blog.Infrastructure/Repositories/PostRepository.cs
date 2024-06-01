using Blog.Domain.Entities;
using Blog.Domain.EntityErrors;
using Blog.Domain.Interfaces;
using Blog.Infrastructure.Persistence;
using Blog.SharedResources;
using Microsoft.EntityFrameworkCore;

namespace Blog.Infrastructure.Repositories;

public class PostRepository(ApplicationDbContext _applicationDbContext) : IPostRepository
{
    public async Task<Result> CreatePost(Post post, CancellationToken cancellationToken)
    {
        if (await EnsurePostFriendlyUrlIsUnique(post.FriendlyUrl!, cancellationToken))
            return Result.Failure(PostErrors.PostWithSameFriendlyUrlExists(post.FriendlyUrl!));

        await _applicationDbContext.Posts.AddAsync(post, cancellationToken);

        await _applicationDbContext.SaveChangesAsync(cancellationToken);

        return Result.Success(Success.Created(post));
    }

    public async Task<Result> DeletePost(int postId, CancellationToken cancellationToken)
    {
        var post = await GetPostByIdAsync(postId, cancellationToken);
        if (post == null)
            return Result.Failure(PostErrors.PostNotFound(postId));

        _applicationDbContext.Posts.Remove(post);

        await _applicationDbContext.SaveChangesAsync(cancellationToken);

        return Result.Success(Success.NoContent());
    }

    public async Task<Result> GetPosts(int pageIndex, int pageSize, CancellationToken cancellationToken)
    {
        var posts = await _applicationDbContext.Posts
            .AsNoTracking()
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return Result.Success(Success.Ok(posts));   
    }

    public async Task<Result> UpdatePost(Post post, CancellationToken cancellationToken)
    {
        var entity = await GetPostByIdAsync(post.Id, cancellationToken);
        if (entity == null)
            return Result.Failure(PostErrors.PostNotFound(post.Id));

        if(entity.FriendlyUrl != post.FriendlyUrl && await EnsurePostFriendlyUrlIsUnique(post.FriendlyUrl!, cancellationToken))
            return Result.Failure(PostErrors.PostWithSameFriendlyUrlExists(post.FriendlyUrl!));

        entity.FriendlyUrl = post.FriendlyUrl;
        entity.Content = post.Content;
        entity.Title = post.Title;

        await _applicationDbContext.SaveChangesAsync(cancellationToken);

        return Result.Success(Success.Ok(entity));
    }

    public async Task<Result> ImportPosts(List<Post> posts, CancellationToken cancellationToken)
    {
        if (posts == null || posts.Count < 1)
            return Result.Failure(PostErrors.NoPostFoundToImport());

        var friendlyUrlList = posts.Select(x => x.FriendlyUrl).ToList();

        var existingPostsWithSameFriendlyUrl = await _applicationDbContext.Posts
            .AsNoTracking()
            .Where(x => friendlyUrlList.Contains(x.FriendlyUrl))
            .Select(x => x.FriendlyUrl)
            .ToListAsync(cancellationToken);

        var uniquePosts = posts
            .Where(x => !existingPostsWithSameFriendlyUrl.Contains(x.FriendlyUrl))
            .ToList();

        if (uniquePosts.Count < 1)
            return Result.Failure(PostErrors.NoPostWithUniqueFriendlyUrl());

        //var existingPosts = await _applicationDbContext.Posts
        //    .AsNoTracking()
        //    .Select(x => new Post { Id = x.Id, FriendlyUrl = x.FriendlyUrl })
        //    .ToListAsync(cancellationToken);

        //var uniquePosts = posts
        //    .Where(newPost => !existingPosts
        //        .Any(existingPost => existingPost.FriendlyUrl == newPost.FriendlyUrl))
        //    .ToList();

        await _applicationDbContext.Posts.AddRangeAsync(uniquePosts, cancellationToken);
        await _applicationDbContext.SaveChangesAsync(cancellationToken);

        return Result.Success(Success.Ok(uniquePosts));
    }

    private async Task<Post?> GetPostByIdAsync(int postId, CancellationToken cancellationToken)
        => await _applicationDbContext.Posts.FindAsync(keyValues: [postId], cancellationToken: cancellationToken);

    private async Task<bool> EnsurePostFriendlyUrlIsUnique(string friendlyUrl, CancellationToken cancellationToken)
        => await _applicationDbContext.Posts.AsNoTracking().AnyAsync(x => x.FriendlyUrl == friendlyUrl, cancellationToken);
}
