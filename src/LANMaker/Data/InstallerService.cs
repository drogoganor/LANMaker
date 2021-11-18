using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace LANMaker.Data
{
	public class InstallerService
    {
        private readonly ManifestService _manifestService;
        private readonly ConfigurationService _configurationService;

        public InstallerService(ManifestService manifestService, ConfigurationService configurationService)
        {
            _manifestService = manifestService;
            _configurationService = configurationService;
        }

		public async Task InstallGame(ServerGame game, CancellationToken cancellationToken)
        {
            var installPath = Path.Combine(_manifestService.ConfigurationDirectory, game.Name);
            var configuration = await _configurationService.GetConfiguration(cancellationToken);

            if (!await IsGameInstalled(game, installPath, configuration))
            {
                var gameArchiveStream = await DownloadGameArchive(game, cancellationToken);
                await ExtractGame(gameArchiveStream, installPath, game);
                await _configurationService.WriteInstalledGame(game, installPath, cancellationToken);
            }
        }

		private async Task<bool> IsGameInstalled(ServerGame game, string installPath, Configuration configuration)
        {
            // Check if the install directory exists
            if (!Directory.Exists(installPath))
            {
                return false;
            }

            // Check if it's in our local config
            if (!configuration.InstalledGames.Any(installedGame => installedGame.Name == game.Name))
            {
                return false;
            }

            return true;
        }

		private async Task<Stream> DownloadGameArchive(ServerGame game, CancellationToken cancellationToken)
        {
            var memoryStream = new MemoryStream();
            using (var client = new HttpClient())
            {
                using (var result = await client.GetAsync(game.ZipUrl))
                {
                    if (result.IsSuccessStatusCode)
                    {
                        var stream = await result.Content.ReadAsStreamAsync(cancellationToken);
                        await stream.CopyToAsync(memoryStream, cancellationToken);
                    }

                }
            }

            return memoryStream;
        }

        private async Task ExtractGame(Stream stream, string installPath, ServerGame game)
        {
            if (!Directory.Exists(installPath))
            {
                Directory.CreateDirectory(installPath);
            }

            using var zip = new ZipArchive(stream);
            zip.ExtractToDirectory(installPath);
            stream.Close();
        }
	}
}
