using System;
using System.Collections.Generic;
using System.Text;

namespace Plugin.Abstraction.Plugins.Models
{
    public class PluginDependencyAttribute : Attribute
    {
        public string[] PluginNames { get; set; }

        public PluginDependencyAttribute(params string[] pluginNames)
        {
            PluginNames = pluginNames;
        }
    }
}
