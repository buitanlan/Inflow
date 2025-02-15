namespace Inflow.Shared.Infrastructure.Modules;

internal class ModuleRequestRegistration(Type requestType, Type responseType, Func<object, CancellationToken, Task<object>> action)
{
    public Type RequestType { get;} = requestType;
    public Type ResponseType { get; } = responseType;
    public Func<object, CancellationToken, Task<object>> Action { get; } = action;
}
