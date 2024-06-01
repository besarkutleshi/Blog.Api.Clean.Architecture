using FluentValidation;
using static Blog.Application.Features.Posts.Commands.ImportPostsFromExcel;

namespace Blog.Application.Features.Posts.Validators;

public class ImportPostsFromExcelValidator : AbstractValidator<ImportPostsFromExcelCommand>
{
    public ImportPostsFromExcelValidator()
    {
        RuleFor(x => x.File)
            .NotNull().WithMessage("File should not be empty.");
        RuleFor(x => x.File.Length)
            .GreaterThan(0).WithMessage("File length should be greater than 0.")
            .When(x => x.File != null);
    }
}
