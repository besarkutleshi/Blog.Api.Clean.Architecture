using Blog.SharedResources;

namespace Blog.Domain.EntityErrors;

public static class PostErrors
{
    public static Error PostNotFound(int postId) 
        => new("Post.NotFound", [$"Post with id = '{postId}' was not found."], ErrorType.NotFound);

    public static Error PostWithSameFriendlyUrlExists(string friendlyUrl)
        => new("Unique.Error", [$"A post with same FriendlyUrl already exists."], ErrorType.Conflict);

    public static Error NoPostFoundToImport()
        => new("Import.Validation.Error", ["There is no post to import."], ErrorType.Validation);

    public static Error NoPostWithUniqueFriendlyUrl()
        => new("Import.Validation.Error", ["There is no post with unique FriendlyUrl to import."], ErrorType.Validation);

    public static Error InvalidExcelData()
        => new("Import.Validation.Error", ["There are invalid excel data."], ErrorType.Validation);
}
