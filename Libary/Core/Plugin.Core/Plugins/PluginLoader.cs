using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Plugin.Abstraction.Plugins;
using Plugin.Abstraction.Plugins.Models;
using Plugin.Abstraction.Plugins.Settings;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime;
using System.Text;

namespace Plugin.Core.Plugins
{
    public class PluginLoader
    {
        private readonly IServiceCollection _services;
        private readonly IConfiguration _config;
        private readonly PluginSettings _settings;
        private static string _executingDir;
        private static List<PluginDef> _plugins = new List<PluginDef>();
        private static List<IPlugin> _modules = new List<IPlugin>();
        public PluginLoader(IServiceCollection services,
       IConfiguration config,
       PluginSettings settings)
        {
            _services = services;
            _config = config;
            _settings = settings;
        }

        public void Load(Action<Assembly> loaded, string? plugin = null)
        {
            _executingDir = Directory.GetParent(Assembly.GetEntryAssembly().Location).FullName;

            _settings.Assemblies.ToList().ForEach(assemblyName =>
            {
                if (plugin != null && plugin != assemblyName)
                {
                    return;
                }

                var assemblyPath = Path.Combine(_executingDir, assemblyName + ".dll");
                if (File.Exists(assemblyPath))
                {
                    var assembly = Assembly.Load(assemblyName);

                    var modules = assembly.GetTypes()
                        .Where(x => x.GetInterface(nameof(IPlugin)) != null)
                        .Select(x => Activator.CreateInstance(x) as IPlugin)
                        .ToList();

                    foreach (var module in modules)
                    {
                        if (module != null & _plugins.Exists(x => x.Id == module.Id))
                        {
                            continue;
                        }

                        InitModule(assemblyName, module);
                    }

                    loaded(assembly);
                }
                else
                {
                    Console.WriteLine($"Can't find assemble {assemblyPath}.");
                }
            });
        }
        private void InitModule(string assembly, IPlugin module)
        {
            module.RegisterDI(_services, _config);
            // string classSummary = GetSummaryComment(module.GetType());
            var name = string.IsNullOrEmpty(module.Name) ? module.GetType().Name : module.Name;
            _modules.Add(module);
            _plugins.Add(new PluginDef
            {
                Id = module.Id,
                Name = name,
                Module = module,
                Assembly = assembly,
            });
            Console.Write($"Loaded plugin ");
            Console.Write(name, Color.Green);
            Console.WriteLine($" from {assembly}.");
        }
    }
}
