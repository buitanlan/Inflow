using Microsoft.EntityFrameworkCore;
using Inflow.Modules.Users.Core.Entities;
using Inflow.Modules.Users.Core.Repositories;

namespace Inflow.Modules.Users.Core.DAL.Repositories;

internal class UserRepository(UsersDbContext context) : IUserRepository
{
    private readonly DbSet<User> _users = context.Users;

    public  Task<User> GetAsync(Guid id)
        => _users.Include(x => x.Role).SingleOrDefaultAsync(x => x.Id == id);

    public  Task<User> GetAsync(string email)
        => _users.Include(x => x.Role).SingleOrDefaultAsync(x => x.Email == email);

    public async Task AddAsync(User user)
    {
        await _users.AddAsync(user);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(User user)
    {
        _users.Update(user);
        await context.SaveChangesAsync();
    }
}
