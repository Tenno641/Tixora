using System.Runtime.InteropServices.ComTypes;
using Tixora.Shared.Domain.Common;

namespace Users.Domain.Users;

public sealed class User : Entity
{
    private List<Role> _roles = [];
    public string Email { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string IdentityId { get; private set; }
    public IReadOnlyCollection<Role> Roles => _roles.ToList();

    public static User Create(string email, string firstName, string lastName, string identityId, Guid? id = null)
    {
        User user = new User(email, firstName, lastName, identityId, id);

        user.RaiseDomainEvent(new UserRegisteredEvent(user.Id));
        
        user._roles.Add(Role.Member);

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

        RaiseDomainEvent(new UserProfileUpdatedEvent(Id, FirstName, LastName));
    }
    
    private User(string email, string firstName, string lastName, string identityId, Guid? id = null) : base(id)
    {
        Email = email;
        FirstName = firstName;
        LastName = lastName;
        IdentityId = identityId;
    }

    private User() { }
}
