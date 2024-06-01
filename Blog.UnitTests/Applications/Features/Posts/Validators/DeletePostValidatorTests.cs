using Blog.Application.Features.Posts.Validators;
using static Blog.Application.Features.Posts.Commands.DeletePost;
using Xunit;
using FluentValidation.TestHelper;
using FluentAssertions;

namespace Blog.UnitTests.Applications.Features.Posts.Validators;

public class DeletePostValidatorTests
{
    private readonly DeletePostValidator _validator;

    public DeletePostValidatorTests()
    {
        _validator = new DeletePostValidator();
    }

    [Fact]
    public void Should_Have_Error_When_PostId_Is_Zero()
    {
        var deletePostCommand = new DeletePostCommand(0);

        var result = _validator.TestValidate(deletePostCommand);

        var errors = result.ShouldHaveValidationErrorFor(x => x.PostId)
            .WithErrorMessage("Post Id should be greater than 0.");
        errors.Count().Should().Be(1);
    }

    [Fact]
    public void Should_Have_Error_When_PostId_Is_Less_Than_Zero()
    {
        var deletePostCommand = new DeletePostCommand(-1);

        var result = _validator.TestValidate(deletePostCommand);

        var errors = result.ShouldHaveValidationErrorFor(x => x.PostId)
            .WithErrorMessage("Post Id should be greater than 0.");
        errors.Count().Should().Be(1);
    }

    [Fact]
    public void Should_Not_Have_Error_When_PostId_Is_Greater_Than_Zero()
    {
        var deletePostCommand = new DeletePostCommand(1);

        var result = _validator.TestValidate(deletePostCommand);

        result.ShouldNotHaveValidationErrorFor(x => x.PostId);
    }
}