using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Plugin.Abstraction.Settings
{
    public interface ISettingService
    {
        T Bind<T>(string path) where T : new();
    }
    public class SettingService : ISettingService
    {
        private readonly IServiceProvider _services;
        private readonly IConfiguration _config;
        public SettingService(IServiceProvider services,
           IConfiguration config
           // , ILogger<SettingService> logger
           )
        {
            _services = services;
            _config = config;
        }
        public T Bind<T>(string path) where T : new()
        {
            var settings = new T();
            _config.Bind(path, settings);
            return settings;
        }
    }
}
