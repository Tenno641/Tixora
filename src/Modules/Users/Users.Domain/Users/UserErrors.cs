using ErrorOr;

namespace Users.Domain.Users;

public static class UserErrors
{
    public static Error UserIsNotFound(Guid userId) => Error.NotFound("Users.NotFound", $"The user with the identifier {userId} not found");
    public static Error UserIsNotFound(string identityId) => Error.NotFound("Users.NotFound", $"The user with the IDP identifier {identityId} not found");
    public static Error UserEmailAlreadyExist => Error.NotFound("Users.Register", "this email already exist");
    public static Error UserLoginFailed => Error.NotFound("Users.Login", "Invalid Credentials");
}
