using Inflow.Modules.Payments.Shared.Entities;
using Inflow.Modules.Payments.Shared.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Inflow.Modules.Payments.Core.DAL.Repositories;

internal class CustomerRepository(PaymentsDbContext context) : ICustomerRepository
{
    private readonly DbSet<Customer> _customers = context.Customers;

    public  Task<Customer> GetAsync(Guid id)
        => _customers.SingleOrDefaultAsync(x => x.Id == id);

    public async Task AddAsync(Customer customer)
    {
        await _customers.AddAsync(customer);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Customer customer)
    {
        _customers.Update(customer);
        await context.SaveChangesAsync();
    }
}
