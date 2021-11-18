using System.Collections.Generic;
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
        private readonly DownloadTrackerService _downloadTrackerService;
        public List<GameDownload> GameDownloads { get; private set; } = new List<GameDownload>();

        public InstallerService(ManifestService manifestService, ConfigurationService configurationService, DownloadTrackerService downloadTrackerService)
        {
            _manifestService = manifestService;
            _configurationService = configurationService;
            _downloadTrackerService = downloadTrackerService;
        }

		public async Task InstallGame(ServerGame game, CancellationToken cancellationToken)
        {
            var installPath = Path.Combine(_manifestService.ConfigurationDirectory, game.Name);
            var configuration = _configurationService.Configuration;

            // Don't install games twice
            if (_downloadTrackerService.IsGameInstalling(game))
            {
                return;
            }

            if (!await IsGameInstalled(game, installPath, configuration))
            {
                var gameArchiveStream = await DownloadGameArchive(game, cancellationToken);

                _downloadTrackerService.TrackGameInstall(game, cancellationToken);
                await ExtractGame(gameArchiveStream, installPath, game, cancellationToken);
                await _configurationService.WriteInstalledGame(game, installPath, cancellationToken);

                // Stop tracking game download
                _downloadTrackerService.RemoveDownload(game);
            }
        }

        public async Task DeleteGame(ClientGame game, CancellationToken cancellationToken)
        {
            var installPath = Path.Combine(_manifestService.ConfigurationDirectory, game.Name);
            try
            {
                Directory.Delete(installPath, true);
            }
            catch
            {
                throw;
            }

            _configurationService.DeleteGame(game, cancellationToken);
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
                _downloadTrackerService.TrackGameDownload(game, client, cancellationToken);

                using (var result = await client.GetAsync(game.ZipUrl, cancellationToken))
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

        private async Task ExtractGame(Stream stream, string installPath, ServerGame game, CancellationToken cancellationToken)
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
