using System.Net.Http.Json;

namespace Users.Infrastructure.Identity;

public class KeyCloakClient
{
    private readonly HttpClient _httpClient;
    
    public KeyCloakClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string> RegisterUserAsync(UserRepresentation userRepresentation, CancellationToken cancellationToken = default)
    {
        HttpResponseMessage httpResponseMessage = await _httpClient.PostAsJsonAsync("users", userRepresentation, cancellationToken);

        httpResponseMessage.EnsureSuccessStatusCode();

        return ExtractUserIdentityId(httpResponseMessage);
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
}