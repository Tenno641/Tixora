using Microsoft.EntityFrameworkCore;
using Tickets.Application.Common;
using Tickets.Domain.Customers;

namespace Tickets.Infrastructure.Persistence.Repositories;

internal sealed class CustomerRepository(TicketsDbContext context) : ICustomerRepository
{
    public async Task<Customer?> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Customers.SingleOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public void Insert(Customer customer)
    {
        context.Customers.Add(customer);
    }
}
