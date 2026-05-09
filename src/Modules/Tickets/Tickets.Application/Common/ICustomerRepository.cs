using Tickets.Domain.Customers;

namespace Tickets.Application.Common;

public interface ICustomerRepository
{
    Task<Customer?> GetAsync(Guid id, CancellationToken cancellationToken = default);

    void Insert(Customer customer);
}
