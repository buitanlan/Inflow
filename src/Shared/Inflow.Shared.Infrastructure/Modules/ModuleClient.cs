using Inflow.Shared.Abstractions.Modules;

namespace Inflow.Shared.Infrastructure.Modules;

internal sealed class ModuleClient(IModuleRegistry moduleRegistry, IModuleSerializer moduleSerializer) : IModuleClient
{
    public async Task<TResult> SendAsync<TResult>(string path, object request, CancellationToken cancellationToken = default) where TResult : class
    {
        var registration = moduleRegistry.GetRequestRegistration(path);
        if (registration is null)
        {
            throw new InvalidOperationException($"No action has been definded for path : {path}");
        }

        var receiverRequest = TranslateType(request, registration.RequestType);
        var result = await registration.Action(receiverRequest, cancellationToken);

        return result is null ? null : TranslateType<TResult>(result);
    }

    public async Task PublishAsync(object message, CancellationToken cancellationToken = default)
    {
        var key = message.GetType().Name;
        var registrations = moduleRegistry
            .GetBroadcastRegistration(key)
            .Where(x => x.ReceiverType != message.GetType());
        var tasks = new List<Task>();
        foreach (var registration in registrations)
        {
            var receiverMessage = TranslateType(message, registration.ReceiverType);
            tasks.Add(registration.Action(receiverMessage, cancellationToken));
        }

        await Task.WhenAll(tasks);
    }

    private T TranslateType<T>(object value)
        => moduleSerializer.Deserialize<T>(moduleSerializer.Serialize(value));

    private object TranslateType(object value, Type type)
        => moduleSerializer.Deserialize(moduleSerializer.Serialize(value), type);

}
