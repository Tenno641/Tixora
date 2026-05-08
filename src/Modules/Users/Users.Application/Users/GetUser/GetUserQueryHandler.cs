using System.Data.Common;
using Dapper;
using ErrorOr;
using MediatR;
using Tixora.Shared.Application.Common;
using Users.Domain.Users;

namespace Users.Application.Users.GetUser;

public sealed record GetUserQuery(Guid UserId) : IRequest<ErrorOr<UserResponse>>;

internal sealed class GetUserQueryHandler : IRequestHandler<GetUserQuery, ErrorOr<UserResponse>>
{
    private readonly IDbConnectionFactory _dbConnectionFactory;
    
    public GetUserQueryHandler(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }
    
    public async Task<ErrorOr<UserResponse>> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        await using DbConnection connection = await _dbConnectionFactory.OpenConnectionAsync();

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

public sealed record UserResponse(Guid Id, string Email, string FirstName, string LastName);
