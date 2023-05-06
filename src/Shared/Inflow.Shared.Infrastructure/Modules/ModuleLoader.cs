using System.Reflection;
using Inflow.Shared.Abstractions.Modules;
using Microsoft.Extensions.Configuration;

namespace Inflow.Shared.Infrastructure.Modules;

public static class ModuleLoader
{
    public static IList<Assembly> LoadAssemblies(IConfiguration configuration)
    {
        const string modulePart = "Inflow.Module.";
        var assemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();
        var locations = assemblies.Where(x => !x.IsDynamic).Select(x => x.Location).ToArray();
        var files = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll")
            .Where(x => !locations.Contains(x, StringComparer.InvariantCultureIgnoreCase))
            .ToList();
        
        var disabledModules = new List<string>();

        foreach (var file in files)
        {
            if(!file.Contains(modulePart)) continue;
            var moduleName = file.Split(modulePart)[1].Split("*")[0].ToLowerInvariant();
            var enabled = configuration.GetValue<bool>($"{moduleName}:module:enabled");

            if (!enabled)
            {
                disabledModules.Add(file);
            }
        }

        files = files.Where(x => !disabledModules.Contains(x)).ToList();
        
        files.ForEach(x => assemblies.Add(AppDomain.CurrentDomain.Load(AssemblyName.GetAssemblyName(x))));
        return assemblies;
    }

    public static IList<IModule> LoadModules(IEnumerable<Assembly> assemblies)
        => assemblies
            .SelectMany(x => x.GetTypes())
            .Where(x => typeof(IModule).IsAssignableFrom(x) && !x.IsInterface)
            .OrderBy(x => x.Name)
            .Select(Activator.CreateInstance)
            .Cast<IModule>()
            .ToList();
}