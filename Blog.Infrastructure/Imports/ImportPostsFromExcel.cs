using Blog.Domain.Entities;
using Blog.Domain.EntityErrors;
using Blog.SharedResources;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;

namespace Blog.Infrastructure.Imports;

public class ImportPostsFromExcel(ILogger<ImportPostsFromExcel> _logger) : IImportPostsFromExcel
{
    public Result ImportDataFromExcel(string filePath, int workSheet, string importedBy)
    {
        try
        {
            List<Post> posts = null!;

            ExcelPackage.LicenseContext = LicenseContext.Commercial;
            using var package = new ExcelPackage(new FileInfo(filePath));

            var excelWorkSheet = GetExcelWorksheet(package);
            if (excelWorkSheet is null)
                return Result.Failure(Error.NotFound("File.NotFound", [$"File with path: '{filePath}' not found."]));

            if (!ValidateExcelFile(package, excelWorkSheet))
                return Result.Failure(PostErrors.InvalidExcelData());

            var worksheet = package.Workbook.Worksheets[workSheet];

            for (int row = 2; row <= worksheet.Dimension.End.Row; row++)
            {
                var post = new Post
                {
                    Title = worksheet.Cells[row, 2].Value.ToString()!,
                    FriendlyUrl = worksheet.Cells[row, 3].Value.ToString(),
                    Content = worksheet.Cells[row, 4].Value.ToString()!,
                    CreatedBy = importedBy
                };

                posts ??= [];
                posts.Add(post);
            }

            return Result.Success(Success.Ok(posts));
        }
        catch (Exception)
        {
            _logger.LogError("Importing Posts from excel failed, path: '{@filePath}' worksheet: '{@workSheet}'",
                filePath,
                workSheet);

            throw;
        }
    }

    private static ExcelWorksheet? GetExcelWorksheet(ExcelPackage package)
        => package.Workbook.Worksheets.FirstOrDefault();

    private static bool ValidateExcelFile(ExcelPackage package, ExcelWorksheet worksheet)
    {
        var headerRow = worksheet.Cells[1, 1, 1, worksheet.Dimension.End.Column];
        var requiredColumns = new[] { "id", "title", "friendlyUrl", "content" };

        foreach (var columnName in requiredColumns)
        {
            if (!headerRow.Any(cell => cell.Text.Equals(columnName, StringComparison.OrdinalIgnoreCase)))
            {
                return false;
            }
        }

        return true;
    }
}
