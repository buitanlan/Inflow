using Inflow.Shared.Abstractions.Events;

namespace Inflow.Modules.Customers.Core.Events;

internal record CustomerLocked(Guid CustomerId) : IEvent;