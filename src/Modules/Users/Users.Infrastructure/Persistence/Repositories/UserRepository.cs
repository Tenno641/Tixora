using Microsoft.EntityFrameworkCore;
using Users.Application.Common;
using Users.Domain.Users;

namespace Users.Infrastructure.Persistence.Repositories;

internal sealed class UserRepository : IUserRepository
{
    private readonly UsersDbContext _dbContext;
    
    public UserRepository(UsersDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<User?> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Users.SingleOrDefaultAsync(u => u.Id == id, cancellationToken);
    }

    public void Insert(User user)
    {
        _dbContext.AttachRange(user.Roles);
        _dbContext.Users.Add(user);
    }
}
