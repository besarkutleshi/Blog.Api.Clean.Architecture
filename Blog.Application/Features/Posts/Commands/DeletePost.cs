using Blog.Domain.Interfaces;
using Blog.SharedResources;
using MediatR;

namespace Blog.Application.Features.Posts.Commands;

public class DeletePost
{
    public record DeletePostCommand(int PostId) : IRequest<Result>;

    public class DeletePostCommandHandler(IPostRepository _postRepository) : IRequestHandler<DeletePostCommand, Result>
    {
        public async Task<Result> Handle(DeletePostCommand request, CancellationToken cancellationToken)
        {
            var result = await _postRepository.DeletePost(request.PostId, cancellationToken);
            if (result.IsSuccess)
                return Result.Success(Success.NoContent());

            return result;
        }
    }
}
