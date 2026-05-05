using System.Data.Common;
using Dapper;
using Events.Application.Common;
using MediatR;

namespace Events.Application.Categories;

public sealed record GetCategoriesQuery : IRequest<List<CategoryResponse>>;

internal sealed class GetCategories : IRequestHandler<GetCategoriesQuery, List<CategoryResponse>>
{
    private readonly IDbConnectionFactory _dbConnectionFactory;
    
    public GetCategories(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }
    
    public async Task<List<CategoryResponse>> Handle(
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
