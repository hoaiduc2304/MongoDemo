using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Plugin.Abstraction.Plugins
{
    public interface IPlugin
    {
        string Id { get; }
        string Name => "";
        void RegisterDI(IServiceCollection services, IConfiguration config);
        int Order => 0;
    }
}
