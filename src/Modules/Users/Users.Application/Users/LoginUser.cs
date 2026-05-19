using MediatR;
using Users.Application.Common;
using ErrorOr;

namespace Users.Application.Users;

public record LoginUserCommand(string Email, string Password): IRequest<ErrorOr<LoginResponse>>;

public class LoginUser: IRequestHandler<LoginUserCommand, ErrorOr<LoginResponse>>
{
    private readonly IIdentityProviderService _identityProviderService;
    
    public LoginUser(IIdentityProviderService identityProviderService)
    {
        _identityProviderService = identityProviderService;
    }
    
    public async Task<ErrorOr<LoginResponse>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        ErrorOr<LoginResponse> loginResult = await _identityProviderService.LoginUserAsync(request.Email, request.Password, cancellationToken);

        if (loginResult.IsError)
            return loginResult.Errors;

        return loginResult.Value;
    }
}