using System.Data.Common;
using Dapper;
using ErrorOr;
using MediatR;
using Tickets.Domain.Customers;
using Tixora.Shared.Application.Common;

namespace Tickets.Application.Customers;

public sealed record GetCustomerQuery(Guid CustomerId) : IRequest<ErrorOr<CustomerResponse>>;

public sealed class GetCustomerById : IRequestHandler<GetCustomerQuery, ErrorOr<CustomerResponse>>
{
    private readonly IDbConnectionFactory _dbConnectionFactory;
    
    public GetCustomerById(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }
    
    public async Task<ErrorOr<CustomerResponse>> Handle(GetCustomerQuery request, CancellationToken cancellationToken)
    {
        await using DbConnection connection = await _dbConnectionFactory.OpenConnectionAsync();

        const string sql =
            """
             SELECT
                 c."Id",
                 c."Email",
                 c."FirstName",
                 c."LastName"
             FROM Tickets."Customers" AS c
             WHERE c."Id" = @CustomerId
             """;

        CustomerResponse? customer = await connection.QuerySingleOrDefaultAsync<CustomerResponse>(sql, request);

        if (customer is null)
            return CustomerErrors.CustomerIsNotFound(request.CustomerId);

        return customer;
    }
}

public sealed record CustomerResponse(Guid Id, string Email, string FirstName, string LastName);
