using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Options;

namespace Users.Infrastructure.Identity;

public class KeyCloakAuthRequestHandler: DelegatingHandler
{
    private readonly KeyCloakOptions _options;
    
    public KeyCloakAuthRequestHandler(IOptions<KeyCloakOptions> options)
    {
        _options = options.Value;
    }
    
    protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        AuthToken authToken = await GetAuthorizationToken(cancellationToken);
        
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", authToken.AccessToken);
        
        HttpResponseMessage response = await base.SendAsync(request, cancellationToken);
        
        response.EnsureSuccessStatusCode();

        return response;
    }

    private async Task<AuthToken> GetAuthorizationToken(CancellationToken cancellationToken)
    {
        KeyValuePair<string, string>[] requestParameters =
        [
            new KeyValuePair<string, string>("grant_type", "client_credentials"),
            new KeyValuePair<string, string>("client_id", _options.ClientId),
            new KeyValuePair<string, string>("client_secret", _options.ClientSecret)
        ];

        using var requestContent = new FormUrlEncodedContent(requestParameters);

        using var authRequest = new HttpRequestMessage(HttpMethod.Post, _options.TokenUrl);
        
        authRequest.Content = requestContent;

        using var httpResponse = await base.SendAsync(authRequest, cancellationToken);
        
        httpResponse.EnsureSuccessStatusCode();
        
        return await httpResponse.Content.ReadFromJsonAsync<AuthToken>(cancellationToken: cancellationToken);
    }
}

public class AuthToken
{
    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; }
}