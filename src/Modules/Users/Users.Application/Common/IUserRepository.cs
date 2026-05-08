using Users.Domain.Users;

namespace Users.Application.Common;

public interface IUserRepository
{
    Task<User?> GetAsync(Guid id, CancellationToken cancellationToken = default);
    void Insert(User user);
}
