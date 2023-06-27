using Microsoft.EntityFrameworkCore;
using Inflow.Modules.Users.Core.Entities;
using Inflow.Modules.Users.Core.Repositories;

namespace Inflow.Modules.Users.Core.DAL.Repositories;

internal class RoleRepository(UsersDbContext context) : IRoleRepository
{
    private readonly DbSet<Role> _roles = context.Roles;

    public async Task<Role> GetAsync(string name) => await _roles.SingleOrDefaultAsync(x => x.Name == name);

    public async Task<IReadOnlyList<Role>> GetAllAsync() => await _roles.ToListAsync();
        
    public async Task AddAsync(Role role)
    {
        await _roles.AddAsync(role);
        await context.SaveChangesAsync();
    }
}
