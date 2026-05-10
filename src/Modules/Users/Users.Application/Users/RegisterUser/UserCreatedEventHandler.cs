using ErrorOr;
using MediatR;
using Tixora.Shared.Application.Common.EventBus;
using Users.Domain.Users;
using Users.IntegrationEvents;

namespace Users.Application.Users.RegisterUser;

public class UserCreatedEventHandler: INotificationHandler<UserRegisteredEvent>
{
    private readonly ISender _sender;
    private readonly IEventBus _eventBus;
    
    public UserCreatedEventHandler(ISender sender, IEventBus eventBus)
    {
        _sender = sender;
        _eventBus = eventBus;
    }
    
    public async Task Handle(UserRegisteredEvent notification, CancellationToken cancellationToken)
    {
        GetUserQuery query = new GetUserQuery(notification.UserId);

        ErrorOr<UserResponse> result = await _sender.Send(query, cancellationToken);

        if (result.IsError)
            throw new Exception(); // TODO: Throw an Eventual Consistency Exception

        await _eventBus.PublishAsync(
            new UserRegisteredIntegrationEvent(
            result.Value.Id,
            result.Value.FirstName,
            result.Value.LastName,
            result.Value.Email));
    }
}