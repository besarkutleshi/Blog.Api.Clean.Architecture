using Blog.Application.Features.Posts.Dtos.Requests;
using Blog.Application.Features.Posts.Validators;
using static Blog.Application.Features.Posts.Commands.UpdatePost;
using Xunit;
using FluentValidation.TestHelper;
using FluentAssertions;

namespace Blog.UnitTests.Applications.Features.Posts.Validators;

public class UpdatePostValidatorTests
{
    private readonly UpdatePostValidator _validator;

    public UpdatePostValidatorTests()
    {
        _validator = new UpdatePostValidator();
    }

    [Fact]
    public void Should_Have_Error_When_Id_Is_Zero()
    {
        var updatePostCommand = new UpdatePostCommand(new UpdatePostDto(0, "Valid Title", "Valid Content", "Valid Friendly Url"));

        var result = _validator.TestValidate(updatePostCommand);

        var errors = result.ShouldHaveValidationErrorFor(x => x.UpdatePostDto.Id)
            .WithErrorMessage("Post Id should be greater than 0.");
        errors.Count().Should().Be(1);
    }

    [Fact]
    public void Should_Have_Error_When_Id_Is_Less_Than_Zero()
    {
        var updatePostCommand = new UpdatePostCommand(new UpdatePostDto(-1, "Valid Title", "Valid Content", "Valid Friendly Url"));

        var result = _validator.TestValidate(updatePostCommand);

        var errors = result.ShouldHaveValidationErrorFor(x => x.UpdatePostDto.Id)
            .WithErrorMessage("Post Id should be greater than 0.");
        errors.Count().Should().Be(1);
    }

    [Fact]
    public void Should_Have_Error_When_Content_Is_Empty()
    {
        var updatePostCommand = new UpdatePostCommand(new UpdatePostDto(0, "Valid Title", string.Empty, "Valid Friendly Url"));

        var result = _validator.TestValidate(updatePostCommand);

        var errors = result.ShouldHaveValidationErrorFor(x => x.UpdatePostDto.Content)
            .WithErrorMessage("Content should not be empty.");
        errors.Count().Should().Be(1);
    }

    [Fact]
    public void Should_Have_Error_When_Content_Exceeds_Maximum_Length()
    {
        var updatePostCommand = new UpdatePostCommand(new UpdatePostDto(1, "Valid Title", new string('a', 1001), "Valid Friendly Url"));

        var result = _validator.TestValidate(updatePostCommand);

        var errors = result.ShouldHaveValidationErrorFor(x => x.UpdatePostDto.Content)
            .WithErrorMessage("Content cannot have more than 1000 characters.");
        errors.Count().Should().Be(1);
    }

    [Fact]
    public void Should_Have_Error_When_Title_Is_Empty()
    {
        var updatePostCommand = new UpdatePostCommand(new UpdatePostDto(1, string.Empty, "Valid Content", "Valid Friendly Url"));

        var result = _validator.TestValidate(updatePostCommand);

        var errors = result.ShouldHaveValidationErrorFor(x => x.UpdatePostDto.Title)
            .WithErrorMessage("Title should not be empty.");
        errors.Count().Should().Be(1);
    }

    [Fact]
    public void Should_Have_Error_When_Title_Exceeds_Maximum_Length()
    {
        var updatePostCommand = new UpdatePostCommand(new UpdatePostDto(1, new string('a', 1001), "Valid Content", "Valid Friendly Url"));

        var result = _validator.TestValidate(updatePostCommand);

        var errors = result.ShouldHaveValidationErrorFor(x => x.UpdatePostDto.Title)
            .WithErrorMessage("Title cannot have more than 100 characters.");
        errors.Count().Should().Be(1);
    }

    [Fact]
    public void Should_Have_Error_When_FriendlyUrl_Exceeds_Maximum_Length()
    {
        var updatePostCommand = new UpdatePostCommand(new UpdatePostDto(1, "Valid Title", "Valid Content", new string('a', 201)));

        var result = _validator.TestValidate(updatePostCommand);

        var errors = result.ShouldHaveValidationErrorFor(x => x.UpdatePostDto.FriendlyUrl)
            .WithErrorMessage("Friendly Url cannot have more than 200 characters.");
        errors.Count().Should().Be(1);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Id_Content_And_Title_Are_Valid()
    {
        var updatePostCommand = new UpdatePostCommand(new UpdatePostDto(1, "Valid Title", "Valid Content", "Valid Friendly Url"));

        var result = _validator.TestValidate(updatePostCommand);

        result.ShouldNotHaveValidationErrorFor(x => x.UpdatePostDto.Id);
        result.ShouldNotHaveValidationErrorFor(x => x.UpdatePostDto.Content);
        result.ShouldNotHaveValidationErrorFor(x => x.UpdatePostDto.Title);
    }
}