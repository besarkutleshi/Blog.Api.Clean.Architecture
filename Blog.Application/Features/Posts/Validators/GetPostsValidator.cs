using FluentValidation;
using static Blog.Application.Features.Posts.Queries.GetPosts;

namespace Blog.Application.Features.Posts.Validators;

public class GetPostsValidator : AbstractValidator<GetPostsQuery>
{
    public GetPostsValidator()
    {
        RuleFor(x => x.PageIndex)
            .GreaterThan(0).WithMessage("Page Index should be greater than 0.");
        RuleFor(x => x.PageSize)
            .ExclusiveBetween(1, 100).WithMessage("Page Size can be only between 1 and 100");
    }
}
