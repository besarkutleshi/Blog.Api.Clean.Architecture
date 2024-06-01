using Blog.Domain.Entities;
using Blog.SharedResources;

namespace Blog.Domain.Interfaces;
public interface IPostRepository
{
    Task<Result> CreatePost(Post post, CancellationToken cancellationToken);
    Task<Result> UpdatePost(Post post, CancellationToken cancellationToken);
    Task<Result> DeletePost(int postId, CancellationToken cancellationToken);
    Task<Result> GetPosts(int pageIndex, int pageSize, CancellationToken cancellationToken);
    Task<Result> ImportPosts(List<Post> posts, CancellationToken cancellationToken);
}

