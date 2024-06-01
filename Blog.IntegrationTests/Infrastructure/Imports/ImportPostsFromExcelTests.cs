using Blog.Domain.Entities;
using Blog.Domain.EntityErrors;
using Blog.Infrastructure.Imports;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Blog.IntegrationTests.Infrastructure.Imports;

public class ImportPostsFromExcelTests : BaseIntegrationTest
{
    private readonly ILogger<ImportPostsFromExcel> _logger;
    private readonly IWebHostEnvironment _env;
    private readonly ImportPostsFromExcel _importPostsFromExcel;

    public ImportPostsFromExcelTests(CustomWebApplicationFactory factory)
        : base(factory)
    {
        _logger = serviceScope.ServiceProvider.GetRequiredService<ILogger<ImportPostsFromExcel>>();    
        _env = factory.Services.GetRequiredService<IWebHostEnvironment>();
        _importPostsFromExcel = new(_logger);
    }

    [Fact]
    public void Should_Have_No_Error_When_Importing_From_Excel()
    {
        string relativePath = Path.Combine("..", "Blog.IntegrationTests\\Uploads", "blog-posts.xlsx");
        string fullPath = Path.Combine(_env.ContentRootPath, relativePath);

        var result = _importPostsFromExcel.ImportDataFromExcel(fullPath, 0, "");

        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Response);
        Assert.NotNull(result.Response.Result);
        Assert.IsType<List<Post>>(result.Response.Result);
    }

    [Fact]
    public void Should_Have_Error_When_Importing_Invalid_Data_From_Excel()
    {
        string relativePath = Path.Combine("..", "Blog.IntegrationTests\\Uploads", "invalid-blog-posts.xlsx");
        string fullPath = Path.Combine(_env.ContentRootPath, relativePath);

        var result = _importPostsFromExcel.ImportDataFromExcel(fullPath, 0, "");

        Assert.NotNull(result);
        Assert.True(result.IsFailure);
        Assert.NotNull(result.Error);
        Assert.Single(result.Error.ErrorMessages);
        Assert.Equal(PostErrors.InvalidExcelData().ErrorMessages[0], result.Error.ErrorMessages[0]);
    }

    [Fact]
    public void Should_Have_Error_When_File_Not_Found()
    {
        string relativePath = Path.Combine("..", "Blog.IntegrationTests\\Uploads", "not-found.xlsx");
        string fullPath = Path.Combine(_env.ContentRootPath, relativePath);

        var result = _importPostsFromExcel.ImportDataFromExcel(fullPath, 0, "");

        Assert.NotNull(result);
        Assert.True(result.IsFailure);
        Assert.NotNull(result.Error);
        Assert.Single(result.Error.ErrorMessages);
        Assert.Equal($"File with path: '{fullPath}' not found.", result.Error.ErrorMessages[0]);
    }
}
