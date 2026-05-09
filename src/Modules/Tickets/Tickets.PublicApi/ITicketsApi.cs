namespace Tickets.PublicApi;

public interface ITicketsApi
{
    Task CreateCustomerAsync(
        Guid customerId,
        string email,
        string firstName,
        string lastName,
        CancellationToken cancellationToken = default);
}
