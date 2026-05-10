using ErrorOr;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Tickets.Application.Customers;
using Users.IntegrationEvents;

namespace Tickets.Infrastructure.IntegrationEvents;

public class UserRegisteredIntegrationEventConsumer: IConsumer<UserRegisteredIntegrationEvent>
{
    private readonly ISender _sender;
    
    public UserRegisteredIntegrationEventConsumer(ISender sender)
    {
        _sender = sender;
    }
    
    public async Task Consume(ConsumeContext<UserRegisteredIntegrationEvent> context)
    {
        CreateCustomerCommand command = new CreateCustomerCommand(context.Message.UserId, 
            context.Message.Email, 
            context.Message.FirstName, 
            context.Message.LastName);

        ErrorOr<Guid> result = await _sender.Send(command);
        
        if (result.IsError)
            throw new Exception(); // TODO: Integration Event Exception
    }
}