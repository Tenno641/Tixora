using System.Data.Common;
using Dapper;
using Events.Application.Common.Mappings;
using Events.Domain.Events;
using MediatR;
using Tixora.Shared.Application.Common;
using ErrorOr;

namespace Events.Application.Events;

public record SearchEventsQuery(string? Title, DateTime? StartAt, DateTime? EndAt, int PageSize, int Page): IRequest<ErrorOr<SearchEventsResponse>>;

public record SearchEventsParameters(string? Title, DateTime? StartAt, DateTime? EndAt, int PageSize, int Page);

public class SearchEvents: IRequestHandler<SearchEventsQuery, ErrorOr<SearchEventsResponse>>
{
    private readonly IDbConnectionFactory _dbConnectionFactory;
    
    public SearchEvents(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }
    
    public async Task<ErrorOr<SearchEventsResponse>> Handle(SearchEventsQuery request, CancellationToken cancellationToken)
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
                         ({string.IsNullOrEmpty(searchEventsParameters.Title)} OR LOWER(e."Title") LIKE LOWER(@Title)) AND
                         ({searchEventsParameters.StartAt is null} OR e."StartAt" = @StartAt) AND
                         ({searchEventsParameters.EndAt is null} OR e."EndAt" = @EndAt) AND
                         e."State" = 1
                     """;
        
        int count = await connection.ExecuteScalarAsync<int>(sql, searchEventsParameters);

        return count;
    }
}

public record SearchEventsResponse(List<EventResponse> Events, int Total, int Page, int PageSize);

public record EventResponse(Guid Id, string Title, string Description, string Location, DateTime StartAt, DateTime EndAt, string State);