using Inflow.Shared.Abstractions.Events;

namespace Inflow.Modules.Users.Core.Events.External;

internal record SignedUp(Guid UserId, string Email, string Role) : IEvent;
