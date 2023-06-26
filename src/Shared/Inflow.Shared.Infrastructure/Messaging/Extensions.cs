using Microsoft.Extensions.DependencyInjection;

namespace Inflow.Shared.Infrastructure.Messaging;

internal static class Extensions
{
    public static IServiceCollection AddMessaging(this IServiceCollection services)
    {
        return services.AddTransient<IMessageBroker, InMemoryMessageBroker>();
    }
}
