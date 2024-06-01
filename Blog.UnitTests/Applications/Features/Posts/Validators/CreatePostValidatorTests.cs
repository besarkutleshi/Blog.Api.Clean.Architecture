using Blog.Application.Features.Posts.Dtos.Requests;
using Blog.Application.Features.Posts.Validators;
using FluentAssertions;
using FluentValidation.TestHelper;
using Xunit;
using static Blog.Application.Features.Posts.Commands.CreatePost;

namespace Blog.UnitTests.Applications.Features.Posts.Validators;

public class CreatePostValidatorTests
{
    private readonly CreatePostValidator _validator;

    public CreatePostValidatorTests()
    {
        _validator = new CreatePostValidator();
    }

    [Fact]
    public void Should_Have_Error_When_Content_Is_Empty()
    {
        var createPostCommand = new CreatePostCommand(new CreatePostDto(string.Empty, "Valid Title", "valid-url"));

        var result = _validator.TestValidate(createPostCommand);

        var errors = result.ShouldHaveValidationErrorFor(x => x.CreatePostDto.Content)
            .WithErrorMessage("Content should not be empty.");
        errors.Count().Should().Be(1);
    }

    [Fact]
    public void Should_Have_Error_When_Content_Exceeds_Maximum_Length()
    {
        var createPostCommand = new CreatePostCommand(new CreatePostDto(new string('a', 1001), "Valid Title", "valid-url"));

        var result = _validator.TestValidate(createPostCommand);

        var errors = result.ShouldHaveValidationErrorFor(x => x.CreatePostDto.Content)
            .WithErrorMessage("Content cannot have more than 1000 characters.");
        errors.Count().Should().Be(1);
    }

    [Fact]
    public void Should_Have_Error_When_Title_Is_Empty()
    {
        var createPostCommand = new CreatePostCommand(new CreatePostDto("Valid content", string.Empty, "valid-url"));

        var result = _validator.TestValidate(createPostCommand);

        var errors = result.ShouldHaveValidationErrorFor(x => x.CreatePostDto.Title)
            .WithErrorMessage("Title should not be empty.");
        errors.Count().Should().Be(1);
    }

    [Fact]
    public void Should_Have_Error_When_Title_Exceeds_Maximum_Length()
    {
        var createPostCommand = new CreatePostCommand(new CreatePostDto("Valid content", new string('a', 101), "valid-url"));

        var result = _validator.TestValidate(createPostCommand);

        var errors = result.ShouldHaveValidationErrorFor(x => x.CreatePostDto.Title)
            .WithErrorMessage("Title cannot have more than 100 characters.");
        errors.Count().Should().Be(1);
    }

    [Fact]
    public void Should_Have_Error_When_FriendlyUrl_Exceeds_Maximum_Length()
    {
        var createPostCommand = new CreatePostCommand(new CreatePostDto("Valid content", "Valid title", new string('a', 201)));

        var result = _validator.TestValidate(createPostCommand);

        var errors = result.ShouldHaveValidationErrorFor(x => x.CreatePostDto.FriendlyUrl)
            .WithErrorMessage("Friendly Url cannot have more than 200 characters.");
        errors.Count().Should().Be(1);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Content_Title_And_FriendlyUrl_Are_Valid()
    {
        var createPostCommand = new CreatePostCommand(new CreatePostDto("Valid content", "Valid title", "Valid friendly url"));

        var result = _validator.TestValidate(createPostCommand);

        result.ShouldNotHaveValidationErrorFor(x => x.CreatePostDto.Content);
        result.ShouldNotHaveValidationErrorFor(x => x.CreatePostDto.Title);
        result.ShouldNotHaveValidationErrorFor(x => x.CreatePostDto.FriendlyUrl);
    }
}