using Dapper;
using ErrorOr;
using MediatR;
using Tixora.Shared.Application.Common;
using Users.Domain.Users;

namespace Users.Application.Users;

public sealed record GetUserQuery(Guid UserId) : IRequest<ErrorOr<UserResponse>>;

public sealed class GetUser : IRequestHandler<GetUserQuery, ErrorOr<UserResponse>>
{
    private readonly IDbConnectionFactory _dbConnectionFactory;
    
    public GetUser(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }
    
    public async Task<ErrorOr<UserResponse>> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        await using var connection = await _dbConnectionFactory.OpenConnectionAsync();

        const string sql =
            """
             SELECT
                 e."Id",
                 e."FirstName",
                 e."LastName",
                 e."Email"
             FROM users."Users" AS e
             WHERE e."Id" = @UserId
             """;

        UserResponse? user = await connection.QuerySingleOrDefaultAsync<UserResponse>(sql, request);

        if (user is null)
            return UserErrors.UserIsNotFound(request.UserId);

        return user;
    }
}

public class UserResponse
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
};
