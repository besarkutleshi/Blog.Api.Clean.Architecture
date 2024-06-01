using Microsoft.AspNetCore.Http;

namespace Blog.Infrastructure.Files;

public interface IFileSaver
{
    string SaveFile(IFormFile file);
}
