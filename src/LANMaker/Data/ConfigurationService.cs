using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace LANMaker.Data
{
	public class ConfigurationService
	{
		private static string ConfigPath => Path.Combine(Directory.GetParent(AppContext.BaseDirectory).FullName, "Resources/config.json");
		private readonly StateContainer state;

		public ConfigurationService(StateContainer state)
		{
			this.state = state;
		}

		public static async Task<Configuration> GetConfiguration(CancellationToken cancellationToken)
		{
			try
			{
				using var stream = new FileStream(ConfigPath, FileMode.Open, FileAccess.Read);
				return await JsonSerializer.DeserializeAsync<Configuration>(stream, cancellationToken: cancellationToken);
			}
			catch
			{
				throw;
			}
		}

		public async void DeleteGame(ClientGame game, CancellationToken cancellationToken)
		{
			var configuration = state.Configuration;

			var installedGame = configuration.InstalledGames.FirstOrDefault(installedGame => installedGame.Name == game.Name);
			if (installedGame != null)
            {
				var installedGames = configuration.InstalledGames.ToList();
				installedGames.Remove(installedGame);

				configuration.InstalledGames = installedGames.ToArray();
				await SaveConfiguration(configuration);
			}
		}

		public async Task WriteInstalledGame(ServerGame game, string installPath, CancellationToken cancellationToken)
		{
			var configuration = state.Configuration;
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

		public async Task SaveConfiguration(Configuration newConfiguration)
		{
			try
			{
				var json = JsonSerializer.Serialize(newConfiguration, new JsonSerializerOptions
                {
					AllowTrailingCommas = true,
					WriteIndented = true,
                });

                using var configFile = new StreamWriter(ConfigPath);
                await configFile.WriteAsync(json);

				// Update state with new configuration
				state.Configuration = newConfiguration;
			}
			catch
			{
				throw;
			}
		}
	}
}
