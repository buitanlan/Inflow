namespace Inflow.Shared.Infrastructure.Modules;

internal interface IModuleRegistry
{
    IEnumerable<ModuleBroadcastRegistration> GetBroadcastRegistration(string key);
    ModuleRequestRegistration GetRequestRegistration(string path);
    void AddBroadcastAction(ModuleBroadcastRegistration registration);
    void AddRequestAction(string path, ModuleRequestRegistration registration);
}
