using Tixora.Shared.Domain.Common;

namespace Users.Domain.Users;

public sealed class User : Entity
{
    public string Email { get; private set; }

    public string FirstName { get; private set; }

    public string LastName { get; private set; }

    public static User Create(string email, string firstName, string lastName)
    {
        var user = new User
        {
            Email = email,
            FirstName = firstName,
            LastName = lastName,
        };

        user.RaiseDomainEvent(new UserRegisteredDomainEvent(user.Id));

        return user;
    }

    public void Update(string firstName, string lastName)
    {
        if (FirstName == firstName && LastName == lastName)
        {
            return;
        }

        FirstName = firstName;
        LastName = lastName;

        RaiseDomainEvent(new UserProfileUpdatedDomainEvent(Id, FirstName, LastName));
    }
    
    private User() { }
}
