using Inflow.Modules.Customers.Core.Domain.Repositories;
using Inflow.Modules.Customers.Core.Events;
using Inflow.Modules.Customers.Core.Exceptions;
using Microsoft.Extensions.Logging;
using Inflow.Shared.Abstractions.Commands;
using Inflow.Shared.Abstractions.Messaging;

namespace Inflow.Modules.Customers.Core.Commands.Handlers;

internal sealed class LockCustomerHandler(
    ICustomerRepository customerRepository,
    IMessageBroker messageBroker,
    ILogger<LockCustomer> logger) : ICommandHandler<LockCustomer>
{

    public async Task HandleAsync(LockCustomer command, CancellationToken cancellationToken = default)
    {
        var customer = await customerRepository.GetAsync(command.CustomerId);
        if (customer is null)
        {
            throw new CustomerNotFoundException(command.CustomerId);
        }
            
        customer.Lock(command.Notes);
        await customerRepository.UpdateAsync(customer);
        await messageBroker.PublishAsync(new CustomerLocked(command.CustomerId), cancellationToken);
        logger.LogInformation($"Locked a customer with ID: '{command.CustomerId}'.");
    }
}
