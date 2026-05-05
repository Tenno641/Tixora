using System.Data.Common;
using Dapper;
using Events.Application.Common;
using Events.Application.Common.Contracts.Events;
using Events.Application.Common.Contracts.Mappings;
using Events.Domain.Events;
using MediatR;

namespace Events.Application.Events;

public record SearchEventsQuery(string? Title, DateTime? StartAt, DateTime? EndAt, int PageSize, int Page): IRequest<SearchEventsResponse>;

public record SearchEventsParameters(string? Title, DateTime? StartAt, DateTime? EndAt, int PageSize, int Page);

public class SearchEvents: IRequestHandler<SearchEventsQuery, SearchEventsResponse>
{
    private readonly IDbConnectionFactory _dbConnectionFactory;
    
    public SearchEvents(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }
    
    public async Task<SearchEventsResponse> Handle(SearchEventsQuery request, CancellationToken cancellationToken)
    {
        await using var connection = await _dbConnectionFactory.OpenConnectionAsync();

        SearchEventsParameters searchEventsParameters = new SearchEventsParameters(
            Title: request.Title,
            StartAt: request.StartAt,
            EndAt: request.EndAt,
            PageSize: request.PageSize,
            Page: (request.Page - 1) * request.PageSize
            );
        
        string sql = $"""
                      SELECT *
                      FROM events."Events" as e
                      WHERE
                          ({searchEventsParameters.Title is null} OR LOWER(e."Title") LIKE LOWER(@Title)) AND
                          ({searchEventsParameters.StartAt is null} OR e."StartAt" = @StartAt) AND
                          ({searchEventsParameters.EndAt is null} OR e."EndAt" = @EndAt) AND
                          e."State" = 1
                      LIMIT @PageSize
                      OFFSET @Page
                      """;

        List<EventResponse> events = (await connection.QueryAsync<Event>(sql, searchEventsParameters))
            .Select(e => e.ToResponse())
            .ToList();

        int count = await CountTotal(connection, searchEventsParameters);
        
        return new SearchEventsResponse(events, count, request.Page, request.PageSize);
    }

    private async Task<int> CountTotal(DbConnection connection, SearchEventsParameters searchEventsParameters)
    {
        string sql = $"""
                     SELECT COUNT(*)
                     FROM events."Events" as e
                     WHERE
                         ({searchEventsParameters.Title is null} OR LOWER(e."Title") LIKE LOWER(@Title)) AND
                         ({searchEventsParameters.StartAt is null} OR e."StartAt" = @StartAt) AND
                         ({searchEventsParameters.EndAt is null} OR e."EndAt" = @EndAt) AND
                         e."State" = 1
                     """;
        
        int count = await connection.ExecuteScalarAsync<int>(sql, searchEventsParameters);

        return count;
    }
}

public record SearchEventsResponse(List<EventResponse> Events, int Total, int Page, int PageSize);