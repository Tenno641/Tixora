using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Users.Domain.Users;

namespace Users.Infrastructure.Persistence.Configurations;

internal sealed class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.HasKey(r => r.Name);
        builder.Property(r => r.Name).HasMaxLength(50);

        builder
            .HasMany<User>()
            .WithMany(u => u.Roles);

        builder.HasData(Role.Member, Role.Administrator);
    }
}
