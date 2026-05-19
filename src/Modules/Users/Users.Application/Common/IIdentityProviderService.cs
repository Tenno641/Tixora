using System.Text.Json.Serialization;

namespace Users.Application.Common;
using ErrorOr;

public interface IIdentityProviderService
{
    Task<ErrorOr<string>> RegisterUserAsync(UserModel userModel, CancellationToken cancellationToken = default);
    Task<ErrorOr<LoginResponse>> LoginUserAsync(string email, string password, CancellationToken cancellationToken = default);
}

public record UserModel(string FirstName, string LastName, string Email, string Password);

public class LoginResponse
{
    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; }
    [JsonPropertyName("expires_in")]
    public int ExpiresIn { get; set; }
    [JsonPropertyName("refresh_token")]
    public string RefreshToken { get; set; }
    [JsonPropertyName("refresh_expires_in")]
    public int RefreshExpiresIn { get; set; }
    [JsonPropertyName("id_token")]
    public string IdToken { get; set; }
}
