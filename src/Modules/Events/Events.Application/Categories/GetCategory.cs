using System.Data.Common;
using Dapper;
using ErrorOr;
using Events.Application.Common;
using Events.Domain.Categories;
using MediatR;

namespace Events.Application.Categories;

public sealed record GetCategoryQuery(Guid CategoryId) : IRequest<ErrorOr<CategoryResponse>>;

internal sealed class GetCategory : IRequestHandler<GetCategoryQuery, ErrorOr<CategoryResponse>>
{
    private readonly IDbConnectionFactory _dbConnectionFactory;
    
    public GetCategory(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task<ErrorOr<CategoryResponse>> Handle(GetCategoryQuery request, CancellationToken cancellationToken)
    {
        await using DbConnection connection = await _dbConnectionFactory.OpenConnectionAsync();

        const string sql =
            $"""
             SELECT
                 "Id",
                 "Name",
                 "IsArchived"
             FROM events."Categories" AS c
             WHERE "Id" = @CategoryId
             """;

        CategoryResponse? category = await connection.QuerySingleOrDefaultAsync<CategoryResponse>(sql, request);

        if (category is null)
            return CategoryErrors.CategoryNotFound;
        
        return category;
    }
}

public sealed record CategoryResponse(Guid Id, string Name, bool IsArchived);
