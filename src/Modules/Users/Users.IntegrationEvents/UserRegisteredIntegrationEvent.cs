using Tixora.Shared.Application.Common.EventBus;

namespace Users.IntegrationEvents;

public class UserRegisteredIntegrationEvent: IntegrationEvent
{
    public Guid UserId { get; }
    public string FirstName { get; }
    public string LastName { get; }
    public string Email { get; }

    public UserRegisteredIntegrationEvent(Guid userId, string firstName, string lastName, string email)
    {
        UserId = userId;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
    }
}