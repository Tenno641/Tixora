using System.Net;
using System.Text.Json.Serialization;
using ErrorOr;
using Microsoft.Extensions.Logging;
using Users.Application.Common;
using Users.Domain.Users;

namespace Users.Infrastructure.Identity;

public class IdentityProviderService: IIdentityProviderService
{
    private readonly KeyCloakClient _keyCloakClient;
    private readonly ILogger<IdentityProviderService> _logger;
    
    public IdentityProviderService(KeyCloakClient keyCloakClient, ILogger<IdentityProviderService> logger)
    {
        _keyCloakClient = keyCloakClient;
        _logger = logger;
    }
    
    public async Task<ErrorOr<string>> RegisterUserAsync(UserModel userModel, CancellationToken cancellationToken = default)
    {
        UserRepresentation userRepresentation = new UserRepresentation
        {
            FirstName = userModel.FirstName,
            LastName = userModel.LastName,
            Email = userModel.Email,
            Username = userModel.Email,
            EmailVerified = true, // Development Purposes
            Enabled = true,
            Credentials = [new CredentialRepresentation { Type = "Password", Value = userModel.Password, Temporary = false }]
        };

        try
        {
            string identityId = await _keyCloakClient.RegisterUserAsync(userRepresentation, cancellationToken);

            return identityId;
        }
        catch (HttpRequestException e) when (e.StatusCode == HttpStatusCode.Conflict)
        {
            _logger.LogError("User Registration Failed");

            return UserErrors.UserEmailAlreadyExist;
        }
    }
    
    public async Task<ErrorOr<LoginResponse>> LoginUserAsync(string email, string password, CancellationToken cancellationToken = default)
    {
        try
        {
            LoginResponse loginResponse = await _keyCloakClient.LoginUserAsync(email, password, cancellationToken);

            return loginResponse;
        }
        catch (HttpRequestException e) when (e.StatusCode == HttpStatusCode.Unauthorized)
        {
            _logger.LogError("User Login Failed");

            return UserErrors.UserLoginFailed;
        }
    }
}

public class LoginRepresentation
{
    [JsonPropertyName("username")]
    public string Username { get; set; }
    [JsonPropertyName("password")]
    public string Password { get; set; }
}

public class UserRepresentation
{
    [JsonPropertyName("firstName")]
    public string FirstName { get; set; }
    [JsonPropertyName("lastName")]
    public string LastName { get; set; }
    [JsonPropertyName("email")]
    public string Email { get; set; } 
    [JsonPropertyName("emailVerified")]
    public bool EmailVerified { get; set; }
    [JsonPropertyName("username")]
    public string Username { get; set; }
    [JsonPropertyName("enabled")]
    public bool Enabled { get; set; }
    [JsonPropertyName("credentials")]
    public CredentialRepresentation[] Credentials { get; set; } = [];
}

public class CredentialRepresentation
{
    [JsonPropertyName("type")]
    public string Type { get; set; }
    [JsonPropertyName("value")]
    public string Value { get; set; }
    [JsonPropertyName("temporary")]
    public bool Temporary { get; set; }
}