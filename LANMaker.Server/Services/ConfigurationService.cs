using System.Text.Json;
using LANMaker.Data;
using LANMaker.Server;
using static System.Environment;

namespace LANMaker.Services
{
    public class ConfigurationService
    {
        private readonly StateContainer state;

        public ConfigurationService(StateContainer state)
        {
            this.state = state;
        }

        public async Task<Configuration> GetConfiguration(CancellationToken cancellationToken)
        {
            CreateConfigurationDirectory();

            Configuration configuration;
            if (!LocalConfigurationExists())
            {
                configuration = new Configuration
                {
                    ManifestUrl = "https://drogoganor.net/lan/manifest.json"
                };

                await SaveConfiguration(configuration);
            }
            else
            {
                configuration = await ReadConfiguration(cancellationToken);
            }

            return configuration;
        }

        private static async Task<Configuration> ReadConfiguration(CancellationToken cancellationToken)
        {
            try
            {
                using var stream = new FileStream(Paths.ConfigurationFile, FileMode.Open, FileAccess.Read);
                return await JsonSerializer.DeserializeAsync<Configuration>(stream, cancellationToken: cancellationToken);
            }
            catch
            {
                throw;
            }
        }

        private static void CreateConfigurationDirectory()
        {
            if (!Directory.Exists(Paths.ConfigDirectory))
            {
                try
                {
                    Directory.CreateDirectory(Paths.ConfigDirectory);
                }
                catch
                {
                    throw;
                }
            }
        }

        private static bool LocalConfigurationExists()
        {
            if (!File.Exists(Paths.ConfigurationFile))
            {
                return false;
            }

            return true;
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

                using var configFile = new StreamWriter(Paths.ConfigurationFile);
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
