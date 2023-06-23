using Inflow.Modules.Customers.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Inflow.Modules.Customers.Core.DAL;

internal class CustomersDbContext(DbContextOptions<CustomersDbContext> options) : DbContext(options)
{
    public DbSet<Customer> Customers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasDefaultSchema(nameof(Customers).ToLowerInvariant())
            .ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}
