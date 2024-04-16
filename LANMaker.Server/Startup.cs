using LANMaker.Data;
using LANMaker.Services;

namespace LANMaker.Server
{
    public static class Startup
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            var stateContainer = new StateContainer();

            services.AddSingleton(stateContainer);
            services.AddSingleton<ConfigurationService>();
            return services;
        }
    }
}
