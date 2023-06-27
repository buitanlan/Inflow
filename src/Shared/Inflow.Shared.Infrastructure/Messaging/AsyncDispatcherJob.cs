using Inflow.Shared.Abstractions.Modules;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Inflow.Shared.Infrastructure.Messaging;

internal sealed class AsyncDispatcherJob(
    IMessageChannel messageChannel,
    ILogger<AsyncDispatcherJob> logger,
    IModuleClient moduleClient) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await foreach (var envelope in messageChannel.Reader.ReadAllAsync(stoppingToken))
        {
            try
            {
                await moduleClient.PublishAsync(envelope.Message, stoppingToken);
            }
            catch (Exception exception)
            {
                logger.LogError(exception, exception.Message);
            }
        }
    }
}
