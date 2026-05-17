using System.Net;
using System.Text.Json.Serialization;
using ErrorOr;
using MassTransit.Middleware;
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
    
    public async Task<ErrorOr<string>> RegisterUser(UserModel userModel, CancellationToken cancellationToken = default)
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