using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using static System.Environment;

namespace LANMaker.Data
{
	public class ConfigurationService
	{
		private string configPath => Path.Combine(Directory.GetParent(AppContext.BaseDirectory).FullName, "Resources/config.json");

        public ConfigurationService()
        {
        }

		public async Task WriteInstalledGame(ServerGame game, string installPath, CancellationToken cancellationToken)
		{
			var configuration = await GetConfiguration(cancellationToken);
			if (configuration.InstalledGames.Any(installedGame => installedGame.Name == game.Name))
			{
				throw new Exception($"Game already exists in config: {game.Name}");
			}

			var installedGame = new ClientGame
			{
				Name = game.Name,
				ExePath = Path.Combine(installPath, game.ExeName),
				InstalledVersion = game.Version,
				InstallPath = installPath,
				Multiplayer = game.Multiplayer,
				Portable = game.Portable,
			};

			var installedGames = configuration.InstalledGames.ToList();

			installedGames.Add(installedGame);

			configuration.InstalledGames = installedGames
				.OrderBy(installedGame => installedGame.Name)
				.ToArray();

			await SaveConfiguration(configuration);
		}

		public async Task<Configuration> GetConfiguration(CancellationToken cancellationToken)
		{
			try
            {
				using (var stream = new FileStream(configPath, FileMode.Open, FileAccess.Read))
				{
					var configuration = await JsonSerializer.DeserializeAsync<Configuration>(stream, cancellationToken: cancellationToken);
					return configuration;
				}
            }
			catch
            {
				throw;
			}
		}

		public async Task SaveConfiguration(Configuration configuration)
		{
			try
			{
				var json = JsonSerializer.Serialize(configuration, new JsonSerializerOptions
                {
					AllowTrailingCommas = true,
					WriteIndented = true,
                });

                using var configFile = new StreamWriter(configPath);
                await configFile.WriteAsync(json);
            }
			catch
			{
				throw;
			}
		}
	}
}
