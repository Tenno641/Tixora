namespace Users.Infrastructure.Identity;

public class KeyCloakOptions
{
    public string AdminUrl { get; set; }
    public string ClientId { get; set; }
    public string ClientSecret { get; set; }
    public string TokenUrl { get; set; }
}