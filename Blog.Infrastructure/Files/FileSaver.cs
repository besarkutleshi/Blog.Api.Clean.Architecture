using Microsoft.AspNetCore.Http;

namespace Blog.Infrastructure.Files;

public class FileSaver : IFileSaver
{
    public string SaveFile(IFormFile file)
    {
        var uploadPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(uploadPath);

        var filePath = Path.Combine(uploadPath, file.FileName);

        using var stream = new FileStream(filePath, FileMode.Create);
        
        file.CopyTo(stream);

        return filePath;
    }
}
