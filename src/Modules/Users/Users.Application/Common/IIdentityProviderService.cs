namespace Users.Application.Common;
using ErrorOr;

public interface IIdentityProviderService
{
    Task<ErrorOr<string>> RegisterUser(UserModel userModel, CancellationToken cancellationToken = default);
}

public record UserModel(string FirstName, string LastName, string Email, string Password);