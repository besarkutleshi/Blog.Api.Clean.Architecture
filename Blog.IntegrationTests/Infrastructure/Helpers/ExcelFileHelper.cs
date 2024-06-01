using Microsoft.AspNetCore.Http;
using Moq;
using OfficeOpenXml;

namespace Blog.IntegrationTests.Infrastructure.Helpers;

public class ExcelFileHelper
{
    public static IFormFile CreateIFormFileFromFile()
    {
        var filePath = "C:\\Users\\BesarKutleshi\\Desktop\\Repos\\Blog.Api\\Blog.IntegrationTests\\Uploads\\blog-posts 1.xlsx";

        var fileInfo = new FileInfo(filePath);

        // Read the file into a MemoryStream
        var memoryStream = new MemoryStream();
        using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
        {
            fileStream.CopyTo(memoryStream);
        }

        // Reset the position of the memory stream
        memoryStream.Position = 0;

        // Create a mock IFormFile
        var formFileMock = new Mock<IFormFile>();
        formFileMock.Setup(f => f.FileName).Returns(fileInfo.Name);
        formFileMock.Setup(f => f.Length).Returns(memoryStream.Length);
        formFileMock.Setup(f => f.OpenReadStream()).Returns(memoryStream);
        formFileMock.Setup(f => f.CopyTo(It.IsAny<Stream>())).Callback<Stream>(stream =>
        {
            memoryStream.Position = 0;
            memoryStream.CopyTo(stream);
        });
        formFileMock.Setup(f => f.CopyToAsync(It.IsAny<Stream>(), It.IsAny<CancellationToken>())).Returns<Stream, CancellationToken>((stream, token) =>
        {
            return memoryStream.CopyToAsync(stream);
        });

        return formFileMock.Object;
    }
}
