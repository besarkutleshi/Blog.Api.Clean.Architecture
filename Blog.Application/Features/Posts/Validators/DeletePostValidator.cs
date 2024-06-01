using FluentValidation;
using static Blog.Application.Features.Posts.Commands.DeletePost;

namespace Blog.Application.Features.Posts.Validators;

public class DeletePostValidator : AbstractValidator<DeletePostCommand>
{
    public DeletePostValidator()
    {
        RuleFor(x => x.PostId)
            .GreaterThan(0).WithMessage("Post Id should be greater than 0.");
    }
}
