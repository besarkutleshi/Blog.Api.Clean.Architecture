using FluentValidation;
using static Blog.Application.Features.Posts.Commands.CreatePost;

namespace Blog.Application.Features.Posts.Validators;

public class CreatePostValidator : AbstractValidator<CreatePostCommand>
{
    public CreatePostValidator()
    {
        RuleFor(x => x.CreatePostDto.Content)
            .NotEmpty().WithMessage("Content should not be empty.")
            .MaximumLength(1000).WithMessage("Content cannot have more than 1000 characters.");

        RuleFor(x => x.CreatePostDto.Title)
            .NotEmpty().WithMessage("Title should not be empty.")
            .MaximumLength(100).WithMessage("Title cannot have more than 100 characters.");

        RuleFor(x => x.CreatePostDto.FriendlyUrl)
            .MaximumLength(200).WithMessage("Friendly Url cannot have more than 200 characters.");
    }
}
