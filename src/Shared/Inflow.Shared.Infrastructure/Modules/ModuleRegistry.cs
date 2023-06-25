namespace Inflow.Shared.Infrastructure.Modules;

internal class ModuleRegistry: IModuleRegistry
{
    private readonly Dictionary<string, ModuleRequestRegistration> _requestRegistrations = new();

    public ModuleRequestRegistration GetRequestRegistration(string path)
        => _requestRegistrations.TryGetValue(path, out var registration) ? registration : null;

    public void AddRequestAction(string path, ModuleRequestRegistration registration)
    {
        if (path is null)
        {
            throw new InvalidOperationException("Request path cannot be null");
        }

        if (registration.GetType().Namespace is null)
        {
            throw new InvalidOperationException("Namespace cannot be null");
        }
        _requestRegistrations.Add(path, registration);
    }
}
