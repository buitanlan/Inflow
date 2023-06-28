using Inflow.Modules.Customers.Core.Domain.Repositories;
using Inflow.Modules.Customers.Core.Events;
using Inflow.Modules.Customers.Core.Exceptions;
using Microsoft.Extensions.Logging;
using Inflow.Shared.Abstractions.Commands;
using Inflow.Shared.Abstractions.Time;
using Inflow.Shared.Infrastructure.Messaging;

namespace Inflow.Modules.Customers.Core.Commands.Handlers;

internal sealed class VerifyCustomerHandler(ICustomerRepository customerRepository, IMessageBroker messageBroker,
    IClock clock, ILogger<VerifyCustomerHandler> logger) : ICommandHandler<VerifyCustomer>
{
    private readonly ICustomerRepository _customerRepository = customerRepository;
    private readonly IMessageBroker _messageBroker = messageBroker;
    private readonly IClock _clock = clock;
    private readonly ILogger<VerifyCustomerHandler> _logger = logger;

    public async Task HandleAsync(VerifyCustomer command, CancellationToken cancellationToken = default)
    {
        var customer = await _customerRepository.GetAsync(command.CustomerId);
        if (customer is null)
        {
            throw new CustomerNotFoundException(command.CustomerId);
        }

        customer.Verify(_clock.CurrentDate());
        await _customerRepository.UpdateAsync(customer);
        await _messageBroker.PublishAsync(new CustomerVerified(command.CustomerId), cancellationToken);
        _logger.LogInformation($"Verified a customer with ID: '{command.CustomerId}'.");
    }
}
