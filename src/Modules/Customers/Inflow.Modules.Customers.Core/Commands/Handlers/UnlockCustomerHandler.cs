using Inflow.Modules.Customers.Core.Domain.Repositories;
using Inflow.Modules.Customers.Core.Events;
using Inflow.Modules.Customers.Core.Exceptions;
using Microsoft.Extensions.Logging;
using Inflow.Shared.Abstractions.Commands;
using Inflow.Shared.Infrastructure.Messaging;

namespace Inflow.Modules.Customers.Core.Commands.Handlers;

internal sealed class UnlockCustomerHandler(
    ICustomerRepository customerRepository,
    IMessageBroker messageBroker,
    ILogger<UnlockCustomerHandler> logger) : ICommandHandler<UnlockCustomer>
{
    public async Task HandleAsync(UnlockCustomer command, CancellationToken cancellationToken = default)
    {
        var customer = await customerRepository.GetAsync(command.CustomerId);
        if (customer is null)
        {
            throw new CustomerNotFoundException(command.CustomerId);
        }
            
        customer.Unlock(command.Notes);
        await customerRepository.UpdateAsync(customer);
        await messageBroker.PublishAsync(new CustomerUnlocked(command.CustomerId), cancellationToken);
        logger.LogInformation($"Unlocked a customer with ID: '{command.CustomerId}'.");
    }
}
