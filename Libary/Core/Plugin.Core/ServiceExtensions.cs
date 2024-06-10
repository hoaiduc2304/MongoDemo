using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Plugin.Abstraction.Plugins.Settings;
using Plugin.Abstraction.Settings;
using Plugin.Core.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Plugin.Core
{
    public static class ServiceExtensions
    {

        public static IServiceCollection AddMyServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddScoped<ISettingService, SettingService>();
            RegisterPlugins(services, config);
            return services;
        }


        private static void RegisterPlugins(IServiceCollection services, IConfiguration config)
        {
            var pluginSettings = new PluginSettings();
            config.Bind("PluginLoader", pluginSettings);

            var loader = new PluginLoader(services, config, pluginSettings);
            loader.Load(assembly =>
            {
               
            });

            services.AddSingleton(loader);
        }
    }
}
