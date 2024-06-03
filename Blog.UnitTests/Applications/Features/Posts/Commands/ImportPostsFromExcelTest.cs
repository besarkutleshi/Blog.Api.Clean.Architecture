using Blog.Application.Helpers;
using Blog.Domain.Interfaces;
using Blog.Infrastructure.BackgroundJobs;
using Blog.Infrastructure.Files;
using Blog.Infrastructure.Imports;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Linq.Expressions;
using Xunit;
using static Blog.Application.Features.Posts.Commands.ImportPostsFromExcel;

namespace Blog.UnitTests.Applications.Features.Posts.Commands;

public class ImportPostsFromExcelCommandHandlerTests
{
    private readonly Mock<IPostRepository> _postRepositoryMock;
    private readonly Mock<IImportPostsFromExcel> _importPostsFromExcelMock;
    private readonly Mock<IFileSaver> _fileSaverMock;
    private readonly Mock<IEnqueueJob> _enqueueJobMock;
    private readonly Mock<IRequestService> _requestService;
    private readonly ImportPostsFromExcelCommandHandler _handler;

    public ImportPostsFromExcelCommandHandlerTests()
    {
        _postRepositoryMock = new Mock<IPostRepository>();
        _importPostsFromExcelMock = new Mock<IImportPostsFromExcel>();
        _fileSaverMock = new Mock<IFileSaver>();
        _enqueueJobMock = new Mock<IEnqueueJob>();
        _requestService = new Mock<IRequestService>();
        _handler = new ImportPostsFromExcelCommandHandler(
            _postRepositoryMock.Object,
            _importPostsFromExcelMock.Object,
            _fileSaverMock.Object,
            _enqueueJobMock.Object,
            _requestService.Object
        );
    }

    [Fact]
    public async Task Should_Return_Failure_When_FilePath_Is_Empty()
    {
        var fileMock = new Mock<IFormFile>();
        fileMock.Setup(x => x.Length).Returns(1024);
        var command = new ImportPostsFromExcelCommand(fileMock.Object);
        _fileSaverMock.Setup(x => x.SaveFile(It.IsAny<IFormFile>())).Returns(string.Empty);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().NotBeNull();
        result.Error.ErrorMessages.Should().ContainSingle();
        result.Error.ErrorMessages[0].Should().BeSameAs("There is no file path to save file.");
    }

    [Fact]
    public async Task Should_Return_Failure_When_EnqueueJob_Fails()
    {
        var fileMock = new Mock<IFormFile>();
        fileMock.Setup(x => x.Length).Returns(1024);

        _fileSaverMock.Setup(x => x.SaveFile(It.IsAny<IFormFile>())).Returns("some/file/path");
        _enqueueJobMock.Setup(x => x.Enqueue(It.IsAny<Expression<Func<Task>>>())).Returns(string.Empty);

        var command = new ImportPostsFromExcelCommand(fileMock.Object);
        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().NotBeNull();
        result.Error.ErrorMessages.Should().ContainSingle();
        result.Error.ErrorMessages[0].Should().BeSameAs("Failed to enqueue job for importing posts.");
    }

    [Fact]
    public async Task Should_Return_Success_When_FilePath_And_EnqueueJob_Are_Valid()
    {
        var fileMock = new Mock<IFormFile>();
        fileMock.Setup(x => x.Length).Returns(1024);

        _fileSaverMock.Setup(x => x.SaveFile(It.IsAny<IFormFile>())).Returns("some/file/path");
        _enqueueJobMock.Setup(x => x.Enqueue(It.IsAny<Expression<Func<Task>>>())).Returns("job-id-123");

        var command = new ImportPostsFromExcelCommand(fileMock.Object);
        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        var message = result.Response.Result!.ToString();
        message.Should().Be("Job with id: job-id-123 enqueued successfully");
    }
}