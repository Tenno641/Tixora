using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Tixora.Shared.Infrastructure.Authentication;

public class AuthenticationJwtOptions: IConfigureOptions<JwtBearerOptions>
{
    private readonly IConfiguration _configuration;
    
    public AuthenticationJwtOptions(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public void Configure(JwtBearerOptions options)
    {
        _configuration.GetSection("Authentication").Bind(options);
    }
}