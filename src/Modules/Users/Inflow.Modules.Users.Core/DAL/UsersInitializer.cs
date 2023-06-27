using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Inflow.Modules.Users.Core.Entities;
using Inflow.Shared.Infrastructure;

namespace Inflow.Modules.Users.Core.DAL;

internal sealed class UsersInitializer(UsersDbContext dbContext, ILogger<UsersInitializer> logger) : IInitializer
{
    private readonly HashSet<string> _permissions = new()
    {
        "customers",
        "deposits", "withdrawals",
        "users",
        "transfers", "wallets"
    };

    public async Task InitAsync()
    {
        if (await dbContext.Roles.AnyAsync())
        {
            return;
        }

        await AddRolesAsync();
        await dbContext.SaveChangesAsync();
    }

    private async Task AddRolesAsync()
    {
        await dbContext.Roles.AddAsync(new Role
        {
            Name = "admin",
            Permissions = _permissions
        });
        await dbContext.Roles.AddAsync(new Role
        {
            Name = "user",
            Permissions = new List<string>()
        });

        logger.LogInformation("Initialized roles.");
    }
}
