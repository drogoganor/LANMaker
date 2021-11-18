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
		public Configuration Configuration { get; private set; }
		private string configPath => Path.Combine(Directory.GetParent(AppContext.BaseDirectory).FullName, "Resources/config.json");

		public async void DeleteGame(ClientGame game, CancellationToken cancellationToken)
		{
			if (Configuration == null)
            {
				await GetConfiguration(cancellationToken);
            }

			var installedGame = Configuration.InstalledGames.FirstOrDefault(installedGame => installedGame.Name == game.Name);
			if (installedGame != null)
            {
				var installedGames = Configuration.InstalledGames.ToList();
				installedGames.Remove(installedGame);

				Configuration.InstalledGames = installedGames.ToArray();
				await SaveConfiguration(Configuration);
			}
		}

		public async Task WriteInstalledGame(ServerGame game, string installPath, CancellationToken cancellationToken)
		{
			if (Configuration == null)
			{
				await GetConfiguration(cancellationToken);
			}

			if (Configuration.InstalledGames.Any(installedGame => installedGame.Name == game.Name))
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

			var installedGames = Configuration.InstalledGames.ToList();

			installedGames.Add(installedGame);

			Configuration.InstalledGames = installedGames
				.OrderBy(installedGame => installedGame.Name)
				.ToArray();

			await SaveConfiguration(Configuration);
		}

		public async Task GetConfiguration(CancellationToken cancellationToken)
		{
			try
            {
				using (var stream = new FileStream(configPath, FileMode.Open, FileAccess.Read))
				{
					Configuration = await JsonSerializer.DeserializeAsync<Configuration>(stream, cancellationToken: cancellationToken);
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
