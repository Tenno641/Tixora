using ErrorOr;
using MediatR;
using Users.Application.Users.GetUser;
using Users.PublicApi;
using UserResponse = Users.PublicApi.UserResponse;

namespace Users.Infrastructure.PublicApi;

public class UsersApi: IUsersApi
{
    private readonly ISender _sender;
    
    public UsersApi(ISender sender)
    {
        _sender = sender;
    }
    
    public async Task<UserResponse?> GetAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        GetUserQuery query = new GetUserQuery(userId);

        ErrorOr<Application.Users.GetUser.UserResponse> result = await _sender.Send(query, cancellationToken);

        return result.IsError
            ? null
            : new UserResponse(userId, 
                result.Value.FirstName,
                result.Value.LastName,
                result.Value.Email);
    }
}