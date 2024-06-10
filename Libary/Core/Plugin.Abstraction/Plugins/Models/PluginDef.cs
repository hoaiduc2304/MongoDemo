using System;
using System.Collections.Generic;
using System.Text;

namespace Plugin.Abstraction.Plugins.Models
{
    public class PluginDef
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Assembly { get; set; }
        public IPlugin Module { get; set; }
        public bool Enabled { get; set; }
    }
}
