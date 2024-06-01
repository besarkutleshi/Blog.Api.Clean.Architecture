using Blog.Infrastructure.Files;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Text;

namespace Blog.IntegrationTests.Infrastructure.Files;

public class FileSaverTests
{
    [Fact]
    public void SaveFile_FileIsSavedSuccessfully()
    {
        var fileSaver = new FileSaver();
        var uploadPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(uploadPath);

        var fileName = "testfile.txt";
        var fileContent = "This is a test file.";
        var fileContentBytes = Encoding.UTF8.GetBytes(fileContent);

        var formFileMock = new Mock<IFormFile>();
        formFileMock.Setup(f => f.FileName).Returns(fileName);
        formFileMock.Setup(f => f.Length).Returns(fileContentBytes.Length);
        formFileMock.Setup(f => f.OpenReadStream()).Returns(new MemoryStream(fileContentBytes));
        formFileMock.Setup(f => f.CopyTo(It.IsAny<Stream>())).Callback<Stream>(stream =>
        {
            using var memoryStream = new MemoryStream(fileContentBytes);
            memoryStream.CopyTo(stream);
        });

        var formFile = formFileMock.Object;

        var filePath = fileSaver.SaveFile(formFile);

        Assert.True(File.Exists(filePath));
        var savedFileContent = File.ReadAllText(filePath);
        Assert.Equal(fileContent, savedFileContent);

        if (Directory.Exists(uploadPath))
        {
            Directory.Delete(uploadPath, true);
        }
    }
}
