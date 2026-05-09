namespace Users.PublicApi;

public interface IUsersApi
{
    Task<UserResponse?> GetAsync(Guid userId, CancellationToken cancellationToken = default);
}

public record UserResponse(Guid UserId, string FirstName, string LastName, string Email);