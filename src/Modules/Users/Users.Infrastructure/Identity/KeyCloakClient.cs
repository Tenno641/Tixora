using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Options;
using Users.Application.Common;

namespace Users.Infrastructure.Identity;

public class KeyCloakClient
{
    private readonly HttpClient _httpClient;
    private readonly KeyCloakOptions _options;
    
    public KeyCloakClient(HttpClient httpClient, IOptions<KeyCloakOptions> options)
    {
        _httpClient = httpClient;
        _options = options.Value;
    }

    public async Task<string> RegisterUserAsync(UserRepresentation userRepresentation, CancellationToken cancellationToken = default)
    {
        AuthToken authToken = await GetAuthorizationToken(cancellationToken);

        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, $"{_options.AdminUrl}/users");
        
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", authToken.AccessToken);
        request.Content = JsonContent.Create(userRepresentation);
        
        HttpResponseMessage httpResponseMessage = await _httpClient.SendAsync(request, cancellationToken);

        httpResponseMessage.EnsureSuccessStatusCode();

        return ExtractUserIdentityId(httpResponseMessage);
    }

    public async Task<LoginResponse> LoginUserAsync(string email, string password, CancellationToken cancellationToken = default)
    {
        HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, _options.TokenUrl);

        KeyValuePair<string, string>[] parameters =
        [
            new KeyValuePair<string, string>("grant_type", "password"),
            new KeyValuePair<string, string>("client_id", _options.PublicClientId),
            new KeyValuePair<string, string>("username", email),
            new KeyValuePair<string, string>("password", password),
            new KeyValuePair<string, string>("scope", "openid")
        ];

        FormUrlEncodedContent formParameters = new FormUrlEncodedContent(parameters);
        
        requestMessage.Content = formParameters;
        
        HttpResponseMessage httpResponseMessage = await _httpClient.SendAsync(requestMessage, cancellationToken);
        
        httpResponseMessage.EnsureSuccessStatusCode();
        
        return await httpResponseMessage.Content.ReadFromJsonAsync<LoginResponse>(cancellationToken);
    }
    
    private string ExtractUserIdentityId(HttpResponseMessage httpResponseMessage)
    {
        const string usersUrlSegment = "users/";
        string? locationHeader = httpResponseMessage.Headers.Location?.PathAndQuery;

        if (locationHeader is null)
            throw new InvalidOperationException("Location header is null");

        int userSegmentIndex = locationHeader.IndexOf(usersUrlSegment, StringComparison.InvariantCultureIgnoreCase);

        string identityId = locationHeader.Substring(userSegmentIndex + usersUrlSegment.Length);
        
        return identityId;
    }
    
    private async Task<AuthToken> GetAuthorizationToken(CancellationToken cancellationToken)
    {
        KeyValuePair<string, string>[] requestParameters =
        [
            new KeyValuePair<string, string>("grant_type", "client_credentials"),
            new KeyValuePair<string, string>("client_id", _options.ConfidentialClientId),
            new KeyValuePair<string, string>("client_secret", _options.ClientSecret)
        ];

        using var requestContent = new FormUrlEncodedContent(requestParameters);

        using var authRequest = new HttpRequestMessage(HttpMethod.Post, _options.TokenUrl);
        
        authRequest.Content = requestContent;

        using var httpResponse = await _httpClient.SendAsync(authRequest, cancellationToken);
        
        httpResponse.EnsureSuccessStatusCode();
        
        return await httpResponse.Content.ReadFromJsonAsync<AuthToken>(cancellationToken: cancellationToken);
    }
}

public class AuthToken
{
    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; }
}