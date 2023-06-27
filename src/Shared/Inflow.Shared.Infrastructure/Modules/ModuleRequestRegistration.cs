namespace Inflow.Shared.Infrastructure.Modules;

internal class ModuleRequestRegistration(Type requestType, Type responceType, Func<object, CancellationToken, Task<object>> action)
{
    public Type RequestType { get;} = requestType;
    public Type ResponceType { get; } = responceType;
    public Func<object, CancellationToken, Task<object>> Action { get; } = action;
}
