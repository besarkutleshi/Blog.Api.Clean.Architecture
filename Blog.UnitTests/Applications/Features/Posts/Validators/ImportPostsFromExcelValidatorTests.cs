using Blog.Application.Features.Posts.Validators;
using Microsoft.AspNetCore.Http;
using Moq;
using static Blog.Application.Features.Posts.Commands.ImportPostsFromExcel;
using Xunit;
using FluentValidation.TestHelper;
using FluentAssertions;

namespace Blog.UnitTests.Applications.Features.Posts.Validators;

public class ImportPostsFromExcelValidatorTests
{
    private readonly ImportPostsFromExcelValidator _validator;

    public ImportPostsFromExcelValidatorTests()
    {
        _validator = new ImportPostsFromExcelValidator();
    }

    [Fact]
    public void Should_Have_Error_When_File_Is_Null()
    {
        var command = new ImportPostsFromExcelCommand(null!);

        // Act
        var result = _validator.TestValidate(command);

        var errors = result.ShouldHaveValidationErrorFor(x => x.File)
            .WithErrorMessage("File should not be empty.");
        errors.Count().Should().Be(1);
    }

    [Fact]
    public void Should_Have_Error_When_File_Length_Is_Zero()
    {
        var fileMock = new Mock<IFormFile>();
        fileMock.Setup(f => f.Length).Returns(0);

        var command = new ImportPostsFromExcelCommand(fileMock.Object);

        var result = _validator.TestValidate(command);

        var errors = result.ShouldHaveValidationErrorFor(x => x.File.Length)
            .WithErrorMessage("File length should be greater than 0.");
        errors.Count().Should().Be(1);
    }

    [Fact]
    public void Should_Not_Have_Error_When_File_Is_Valid()
    {
        var fileMock = new Mock<IFormFile>();
        fileMock.Setup(f => f.Length).Returns(1024);

        var command = new ImportPostsFromExcelCommand(fileMock.Object);

        var result = _validator.TestValidate(command);

        result.ShouldNotHaveValidationErrorFor(x => x.File);
        result.ShouldNotHaveValidationErrorFor(x => x.File.Length);
    }
}
