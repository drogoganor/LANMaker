using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using static System.Environment;

namespace LANMaker.Data
{
    public class ManifestService
    {
        public string ConfigurationDirectory => Path.Combine(Environment.GetFolderPath(SpecialFolder.MyDocuments), "LANMaker");
        public string ManifestPath => Path.Combine(ConfigurationDirectory, "manifest.json");

        private readonly ConfigurationService _configurationService;

        public ManifestService(ConfigurationService configurationService)
        {
            _configurationService = configurationService;
        }

        public async Task<Manifest> UpdateLatestManifest(CancellationToken stoppingToken)
        {
            CreateManifestDirectory();

            if (!LocalManifestExists())
            {
                var manifest = await FetchManifest(stoppingToken);
                await SaveManifest(manifest);

                return manifest;
            }

            return await GetManifest();
        }

        public async Task<Manifest> GetManifest()
        {
            try
            {
                using (var stream = new FileStream(ManifestPath, FileMode.Open, FileAccess.Read))
                {
                    var manifest = await JsonSerializer.DeserializeAsync<Manifest>(stream);
                    return manifest;
                }
            }
            catch
            {
                throw;
            }
        }

        public async Task SaveManifest(Manifest manifest)
        {
            try
            {
                var json = JsonSerializer.Serialize(manifest, new JsonSerializerOptions
                {
                    AllowTrailingCommas = true,
                    WriteIndented = true,
                });

                using var manifestFile = new StreamWriter(ManifestPath);
                await manifestFile.WriteAsync(json);
            }
            catch
            {
                throw;
            }
        }

        private void CreateManifestDirectory()
        {
            if (!Directory.Exists(ConfigurationDirectory))
            {
                try
                {
                    Directory.CreateDirectory(ConfigurationDirectory);
                }
                catch
                {
                    throw;
                }
            }
        }

        private bool LocalManifestExists()
        {
            if (!File.Exists(ManifestPath))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Fetch the manifest from the remote source in the configuration.
        /// </summary>
        /// <param name="stoppingToken"></param>
        /// <returns></returns>
        private async Task<Manifest> FetchManifest(CancellationToken stoppingToken)
        {
            var configuration = await _configurationService.GetConfiguration();
            var manifestUrl = configuration.ManifestUrl;
            var manifestStream = await DownloadTextFile(manifestUrl, stoppingToken);
            var manifest = await JsonSerializer.DeserializeAsync<Manifest>(manifestStream, cancellationToken: stoppingToken);
            return manifest;
        }

        private static async Task<Stream> DownloadTextFile(string url, CancellationToken stoppingToken)
        {
            using (var client = new HttpClient())
            {
                using var result = await client.GetAsync(url, stoppingToken);
                if (result.IsSuccessStatusCode)
                {
                    return await result.Content.ReadAsStreamAsync(stoppingToken);
                }
            }

            return null;
        }
    }
}
