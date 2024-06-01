using Blog.Application.Features.Posts.Validators;
using static Blog.Application.Features.Posts.Queries.GetPosts;
using Xunit;
using FluentValidation.TestHelper;
using FluentAssertions;

namespace Blog.UnitTests.Applications.Features.Posts.Validators;

public class GetPostsValidatorTests
{
    private readonly GetPostsValidator _validator;

    public GetPostsValidatorTests()
    {
        _validator = new GetPostsValidator();
    }

    [Fact]
    public void Should_Have_Error_When_PageIndex_Is_Zero()
    {
        var getPostsQuery = new GetPostsQuery(0, 10);

        var result = _validator.TestValidate(getPostsQuery);

        var errors = result.ShouldHaveValidationErrorFor(x => x.PageIndex)
            .WithErrorMessage("Page Index should be greater than 0.");
        errors.Count().Should().Be(1);
    }

    [Fact]
    public void Should_Have_Error_When_PageIndex_Is_Less_Than_Zero()
    {
        var getPostsQuery = new GetPostsQuery(-1, 10);

        var result = _validator.TestValidate(getPostsQuery);

        var errors = result.ShouldHaveValidationErrorFor(x => x.PageIndex)
            .WithErrorMessage("Page Index should be greater than 0.");
        errors.Count().Should().Be(1);
    }

    [Fact]
    public void Should_Have_Error_When_PageSize_Is_Less_Than_One()
    {
        var getPostsQuery = new GetPostsQuery(1, 0);

        var result = _validator.TestValidate(getPostsQuery);

        var errors = result.ShouldHaveValidationErrorFor(x => x.PageSize)
            .WithErrorMessage("Page Size can be only between 1 and 100");
        errors.Count().Should().Be(1);
    }

    [Fact]
    public void Should_Have_Error_When_PageSize_Is_Greater_Than_One_Hundred()
    {
        var getPostsQuery = new GetPostsQuery(1, 101);

        var result = _validator.TestValidate(getPostsQuery);

        var errors = result.ShouldHaveValidationErrorFor(x => x.PageSize)
            .WithErrorMessage("Page Size can be only between 1 and 100");
        errors.Count().Should().Be(1);
    }

    [Fact]
    public void Should_Not_Have_Error_When_PageIndex_And_PageSize_Are_Valid()
    {
        var getPostsQuery = new GetPostsQuery(1, 10);

        var result = _validator.TestValidate(getPostsQuery);

        result.ShouldNotHaveValidationErrorFor(x => x.PageIndex);
        result.ShouldNotHaveValidationErrorFor(x => x.PageSize);
    }
}
