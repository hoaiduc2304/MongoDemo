using DNH.Storage.MongoDB.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Plugin.Abstraction.Plugins;
using ProjectModule.Services;
using Plugin.Abstraction.Settings;
using System.Runtime;
using DNH.Storage.MongoDB.Repository;
using DNH.Storage.MongoDB;
using ProjectModule.Abstraction.Repository;
using ProjectModule.Repository.Repository;

namespace ProjectModule
{
    public class ProjectModulePlugin : IPlugin
    {
        public string Id => "2a0b3567-5a4e-4d75-90a7-7bb0f426354d";
        public string Name => "Project Module";

        public void RegisterDI(IServiceCollection services, IConfiguration config)
        {
           
            MongDBSettings dbsetting = new MongDBSettings();
            config.Bind("Database", dbsetting);

            services.AddSingleton(provider =>
            {
                var settingService = provider.CreateScope().ServiceProvider.GetRequiredService<ISettingService>();
                return settingService.Bind<MongDBSettings>("Database");
            });
            services.AddSingleton<MongoDbContext, MongoDbContext>();
            services.AddScoped(typeof(IMongoRepository<>), typeof(MongoRepository<>));
            services.AddScoped<IProjectRepository, ProjectRepository>();
            services.AddScoped<IProjectService, ProjectService>();
        }
    }
}
