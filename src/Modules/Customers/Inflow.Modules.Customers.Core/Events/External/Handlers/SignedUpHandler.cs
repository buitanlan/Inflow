using Inflow.Modules.Customers.Core.Domain.Entities;
using Inflow.Modules.Customers.Core.Domain.Repositories;
using Inflow.Modules.Customers.Core.Exceptions;
using Inflow.Shared.Abstractions.Events;
using Inflow.Shared.Abstractions.Time;
using Microsoft.Extensions.Logging;

namespace Inflow.Modules.Customers.Core.Events.External.Handlers;

internal sealed class SignedUpHandler(
    ICustomerRepository customerRepository,
    IClock clock,
    ILogger<SignedUpHandler> logger) : IEventHandler<SignedUp>
{
    public async Task HandleAsync(SignedUp @event, CancellationToken cancellationToken = default)
    {
        if (@event.Role is not "user")
        {
            return;
        }

        var customerId = @event.UserId;
        if (await customerRepository.GetAsync(customerId) is not null)
        {
            throw new CustomerAlreadyException(customerId);
        }
        var customer = new Customer(Guid.NewGuid(), @event.Email, clock.CurrentDate());
        await customerRepository.AddAsync(customer);
        logger.LogInformation($"Created a customer with Id: {customer.Id}.");
    }
}
