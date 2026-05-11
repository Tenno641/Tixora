using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tickets.Domain.Customers;
using Tickets.Domain.Orders;

namespace Tickets.Infrastructure.Persistence.Configurations;

internal sealed class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(c => c.Id);

        builder.HasOne<Customer>().WithMany().HasForeignKey(o => o.CustomerId);

        builder.HasMany(o => o.OrderItems).WithOne().HasForeignKey(oi => oi.OrderId);
    }
}
