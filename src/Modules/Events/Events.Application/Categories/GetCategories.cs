using System.Data.Common;
using Dapper;
using MediatR;
using Tixora.Shared.Application.Common;
using ErrorOr;

namespace Events.Application.Categories;

public sealed record GetCategoriesQuery : IRequest<ErrorOr<List<CategoryResponse>>>;

internal sealed class GetCategories : IRequestHandler<GetCategoriesQuery, ErrorOr<List<CategoryResponse>>>
{
    private readonly IDbConnectionFactory _dbConnectionFactory;
    
    public GetCategories(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }
    
    public async Task<ErrorOr<List<CategoryResponse>>> Handle(
        GetCategoriesQuery request,
        CancellationToken cancellationToken)
    {
        await using DbConnection connection = await _dbConnectionFactory.OpenConnectionAsync();

        const string sql =
            """
             SELECT
                 "Id",
                 "Name",
                 "IsArchived"
             FROM events."Categories"
             """;

        List<CategoryResponse> categories = (await connection.QueryAsync<CategoryResponse>(sql, request)).ToList();

        return categories;
    }
}
