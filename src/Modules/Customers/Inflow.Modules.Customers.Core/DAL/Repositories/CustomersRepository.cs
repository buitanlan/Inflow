using Inflow.Modules.Customers.Core.Domain.Entities;
using Inflow.Modules.Customers.Core.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Inflow.Modules.Customers.Core.DAL.Repositories;

internal class CustomersRepository : ICustomerRepository
{
    private readonly CustomersDbContext _context;
    private readonly DbSet<Customer> _customers;

    public CustomersRepository(CustomersDbContext context)
    {
        _context = context;
        _customers = context.Customers;
    }

    public async Task<bool> ExistsAsync(string name) =>await _customers.AnyAsync(x => x.Name == name);
    
    public async Task<Customer> GetAsync(Guid id) => await _customers.SingleOrDefaultAsync(x => x.Id == id);
    

    public async Task AddAsync(Customer customer)
    {
        await _customers.AddAsync(customer);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Customer customer)
    {
        _customers.Update(customer);
        await _context.SaveChangesAsync();
    }
}