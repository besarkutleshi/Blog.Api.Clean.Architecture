using Blog.SharedResources;

namespace Blog.Domain.EntityErrors;

public static class UserErrors
{
    public static Error UserNotFound(string email)
        => new("User.NotFound", [$"User with email '{email}' not found"], ErrorType.NotFound);

    public static Error IncorrectPasswrod() 
        => new("Validation.Error", ["Incorrect Password"], ErrorType.Validation);

    public static Error UserNotFoundById(string id)
        => new("User.NotFound", [$"User with id '{id}' not found"], ErrorType.NotFound);

    public static Error NoRoleAssigned()
        => new("Roles.NotFound", [$"There is no role assigned"], ErrorType.NotFound);
}
