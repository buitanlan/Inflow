using Inflow.Shared.Abstractions.Messaging;
using Microsoft.Extensions.DependencyInjection;

namespace Inflow.Shared.Infrastructure.Messaging;

internal static class Extensions
{
    public static IServiceCollection AddMessaging(this IServiceCollection services)
    {
        services.AddTransient<IMessageBroker, InMemoryMessageBroker>();
        services.AddSingleton<IMessageChannel, MessageChannel>();
        services.AddSingleton<IAsyncMessageDispatcher, AsyncMessageDispatcher>();

        var messagingOptions = services.GetOptions<MessagingOptions>("messaging");
        services.AddSingleton(messagingOptions);

        if (messagingOptions.UseAsyncDispatcher)
        {
            services.AddHostedService<AsyncDispatcherJob>();
        }

        return services;
    }
}
