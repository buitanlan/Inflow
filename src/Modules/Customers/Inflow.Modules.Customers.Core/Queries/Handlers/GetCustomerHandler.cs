using Inflow.Modules.Customers.Core.DAL;
using Inflow.Modules.Customers.Core.DTO;
using Inflow.Shared.Abstractions.Queries;
using Microsoft.EntityFrameworkCore;

namespace Inflow.Modules.Customers.Core.Queries.Handlers;

internal class GetCustomerHandler(CustomersDbContext dbContext) : IQueryHandler<GetCustomer, CustomerDetailsDto>
{
    public async Task<CustomerDetailsDto> HandleAsync(GetCustomer query, CancellationToken cancellationToken = default)
    {
        var customer = await dbContext.Customers
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.Id == query.CustomerId, cancellationToken);
        return customer?.AsDetailsDto();
    }
}
