using FluentValidation;
using static Blog.Application.Features.Posts.Commands.UpdatePost;

namespace Blog.Application.Features.Posts.Validators;

public class UpdatePostValidator: AbstractValidator<UpdatePostCommand>
{
    public UpdatePostValidator()
    {
        RuleFor(x => x.UpdatePostDto.Id)
            .GreaterThan(0).WithMessage("Post Id should be greater than 0.");

        RuleFor(x => x.UpdatePostDto.Content)
            .NotEmpty().WithMessage("Content should not be empty.")
            .MaximumLength(1000).WithMessage("Content cannot have more than 1000 characters.");

        RuleFor(x => x.UpdatePostDto.Title)
            .NotEmpty().WithMessage("Title should not be empty.")
            .MaximumLength(100).WithMessage("Title cannot have more than 100 characters.");

        RuleFor(x => x.UpdatePostDto.FriendlyUrl)
            .MaximumLength(200).WithMessage("Friendly Url cannot have more than 200 characters.");
    }
}
