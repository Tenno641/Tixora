using MediatR;
using Tickets.Application.Customers;
using Tickets.PublicApi;

namespace Tickets.Infrastructure.PublicApi;

public sealed class TicketsApi : ITicketsApi
{
    private readonly ISender _sender;
    
    public TicketsApi(ISender sender)
    {
        _sender = sender;
    }
    
    public async Task CreateCustomerAsync(
        Guid customerId,
        string email,
        string firstName,
        string lastName,
        CancellationToken cancellationToken = default)
    {
        CreateCustomerCommand command = new CreateCustomerCommand(customerId, email, firstName, lastName);
        
        await _sender.Send(command, cancellationToken);
    }
}
