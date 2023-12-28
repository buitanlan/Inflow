using Inflow.Modules.Wallets.Core.Owners.Entities;
using Inflow.Modules.Wallets.Core.Wallets.Entities;
using Microsoft.EntityFrameworkCore;

namespace Inflow.Modules.Wallets.Infrastructure.EF;

internal class WalletsDbContext(DbContextOptions<WalletsDbContext> options) : DbContext(options)
{
    public DbSet<CorporateOwner> CorporateOwners { get; set; }
    public DbSet<IndividualOwner> IndividualOwners { get; set; }
    public DbSet<Transfer> Transfers { get; set; }
    public DbSet<IncomingTransfer> IncomingTransfers { get; set; }
    public DbSet<OutgoingTransfer> OutgoingTransfers { get; set; }
    public DbSet<Wallet> Wallets { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("wallets");
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}
