using Tixora.Shared.Domain.Common;

namespace Tickets.Domain.Customers;

public class Customer: Entity
{
    public string Email { get; private set; }

    public string FirstName { get; private set; }

    public string LastName { get; private set; }

    public static Customer Create(string email, string firstName, string lastName, Guid? id = null)
    {
        Customer customer = new Customer(email, firstName, lastName, id);

        return customer;
    }

    public void Update(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }

    private Customer(string email, string firstName, string lastName, Guid? id = null): base(id)
    {
        Email = email;
        FirstName = firstName;
        LastName = lastName;
    }
    
    private Customer() { }
}