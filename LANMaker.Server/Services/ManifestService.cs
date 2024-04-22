using System.Text.Json;
using LANMaker.Data;
using LANMaker.Server;

namespace LANMaker.Services
{
    public class ManifestService
    {
        private readonly ConfigurationService _configurationService;
        private readonly StateContainer state;

        public ManifestService(StateContainer state, ConfigurationService configurationService)
        {
            this.state = state;
            _configurationService = configurationService;
        }

        public async Task<Manifest> GetManifest(CancellationToken stoppingToken)
        {
            CreateManifestDirectory();

            if (!LocalManifestExists())
            {
                var manifest = await FetchManifest(stoppingToken);
                await SaveManifest(manifest);

                return manifest;
            }

            return await ReadManifest();
        }

        public async Task UpdateManifest(CancellationToken stoppingToken)
        {
            DeleteManifest();
            var manifest = await GetManifest(stoppingToken);
            await SaveManifest(manifest);
        }

        private static async Task<Manifest> ReadManifest()
        {
            try
            {
                using var stream = new FileStream(Paths.ManifestPath, FileMode.Open, FileAccess.Read);
                var manifest = await JsonSerializer.DeserializeAsync<Manifest>(stream);
                return manifest;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Fetch the manifest from the remote source in the configuration.
        /// </summary>
        /// <param name="stoppingToken"></param>
        /// <returns></returns>
        private async Task<Manifest> FetchManifest(CancellationToken stoppingToken)
        {
            var configuration = state.Configuration;
            if (configuration == null)
            {
                return null;
            }

            var manifestUrl = configuration.ManifestUrl;
            var manifestStream = await DownloadTextFile(manifestUrl, stoppingToken);
            Manifest manifest;
            try
            {
                manifest = JsonSerializer.Deserialize<Manifest>(manifestStream);
            }
            catch
            {
                throw;
            }

            return manifest;
        }

        private static void DeleteManifest()
        {
            if (File.Exists(Paths.ManifestPath))
            {
                try
                {
                    File.Delete(Paths.ManifestPath);
                }
                catch
                {
                    throw;
                }
            }
        }

        private async Task SaveManifest(Manifest newManifest)
        {
            try
            {
                var json = JsonSerializer.Serialize(newManifest, new JsonSerializerOptions
                {
                    AllowTrailingCommas = true,
                    WriteIndented = true,
                });

                using var manifestFile = new StreamWriter(Paths.ManifestPath);
                await manifestFile.WriteAsync(json);

                // Update state with new manifest
                state.Manifest = newManifest;
            }
            catch
            {
                throw;
            }
        }

        private static void CreateManifestDirectory()
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

        private static bool LocalManifestExists()
        {
            if (!File.Exists(Paths.ManifestPath))
            {
                return false;
            }

            return true;
        }

        private static async Task<string> DownloadTextFile(string url, CancellationToken stoppingToken)
        {
            using (var client = new HttpClient())
            {
                using var result = await client.GetAsync(url, stoppingToken);
                if (result.IsSuccessStatusCode)
                {
                    return await result.Content.ReadAsStringAsync(stoppingToken);
                }
            }

            return null;
        }
    }
}
