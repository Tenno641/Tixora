using ErrorOr;
using MediatR;
using Tickets.PublicApi;
using Users.Domain.Users;

namespace Users.Application.Users.RegisterUser;

public class UserCreatedEventHandler: INotificationHandler<UserRegisteredEvent>
{
    private readonly ITicketsApi _ticketsApi;
    private readonly ISender _sender;
    
    public UserCreatedEventHandler(ITicketsApi ticketsApi, ISender sender)
    {
        _ticketsApi = ticketsApi;
        _sender = sender;
    }
    
    public async Task Handle(UserRegisteredEvent notification, CancellationToken cancellationToken)
    {
        GetUserQuery query = new GetUserQuery(notification.UserId);

        ErrorOr<UserResponse> result = await _sender.Send(query, cancellationToken);

        if (result.IsError)
            throw new Exception(); // TODO: Throw an Eventual Consistency Exception

        await _ticketsApi.CreateCustomerAsync(notification.UserId,
            result.Value.Email,
            result.Value.FirstName,
            result.Value.LastName,
            cancellationToken);
    }
}