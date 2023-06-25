namespace Inflow.Shared.Infrastructure.Modules;

internal class ModuleRequestRegistration
{
    public Type RequestType { get;}
    public Type ResponceType { get; }
    public Func<object, CancellationToken, Task<object>> Action { get; }

    public ModuleRequestRegistration(Type requestType, Type responceType, Func<object, CancellationToken, Task<object>> action)
    {
        RequestType = requestType;
        ResponceType = responceType;
        Action = action;
    }
}
