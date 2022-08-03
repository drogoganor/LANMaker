using LANMaker.Data;
using LANMaker.Services;

namespace LANMaker
{
	public static class Startup
	{
		public static async Task RegisterLANMaker(this IServiceProvider services)
		{
            // Load settings
            var loadSettingsAdapter = services.GetRequiredService<StartupService>();

            var cancellationToken = new CancellationToken();
            var configuration = await loadSettingsAdapter.GetConfiguration(cancellationToken);
            var manifest = await loadSettingsAdapter.GetManifest(cancellationToken);

            var stateContainer = services.GetRequiredService<StateContainer>();
            stateContainer.Configuration = configuration;
            stateContainer.Manifest = manifest;

            var games = await loadSettingsAdapter.GetCombinedGames(cancellationToken);
            stateContainer.Games = games;
        }
	}
}
