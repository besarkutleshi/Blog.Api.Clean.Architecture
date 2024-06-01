using Blog.SharedResources;

namespace Blog.Infrastructure.Imports;

public interface IImportPostsFromExcel
{
    Result ImportDataFromExcel(string filePath, int workSheet, string importedBy);
}
