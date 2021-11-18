using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;

namespace LANMaker.Data
{
	public class DownloadTrackerService
    {
        public event EventHandler DownloadStatusChanged;
        public List<GameDownload> GameDownloads { get; private set; } = new List<GameDownload>();

        public DownloadTrackerService()
        {

        }

        public bool IsGameInstalling(ServerGame game)
        {
            if (GameDownloads.Any(gameDownload => gameDownload.Game.Name == game.Name))
            {
                return true;
            }

            return false;
        }

        public void TrackGameDownload(ServerGame game, HttpClient client, CancellationToken cancellationToken)
        {
            var gameDownload = new GameDownload
            {
                DownloadTime = DateTime.Now,
                Game = game,
                HttpClient = client,
                CancellationToken = cancellationToken,
                DownloadStatus = GameDownloadStatus.Downloading,
            };

            GameDownloads.Add(gameDownload);
            DownloadStatusChanged?.Invoke(this, null);
        }

        public void TrackGameInstall(ServerGame game, CancellationToken cancellationToken)
        {
            //var originalDateTime = RemoveDownload(game);
            var download = GameDownloads.First(gameDownload => gameDownload.Game.Name == game.Name);
            download.DownloadStatus = GameDownloadStatus.Installing;
            download.HttpClient = null;
            DownloadStatusChanged?.Invoke(this, null);

            //var gameInstall = new GameDownload
            //{
            //    DownloadTime = originalDateTime,
            //    Game = game,
            //    CancellationToken = cancellationToken,
            //    DownloadStatus = GameDownloadStatus.Installing,
            //    HttpClient = null,
            //};

            //GameDownloads.Add(gameInstall);
        }

        public DateTime RemoveDownload(ServerGame game)
        {
            var existingDownload = GetGameDownload(game);
            GameDownloads.Remove(existingDownload);
            DownloadStatusChanged?.Invoke(this, null);
            return existingDownload.DownloadTime;
        }

        public void CancelDownload(GameDownload gameDownload)
        {
            if (gameDownload.CancellationToken.CanBeCanceled)
            {
                gameDownload.CancellationToken.ThrowIfCancellationRequested();
            }

            GameDownloads.Remove(gameDownload);
            DownloadStatusChanged?.Invoke(this, null);
        }

        private GameDownload GetGameDownload(ServerGame game)
        {
            return GameDownloads.FirstOrDefault(gameDownload => gameDownload.Game.Name == game.Name);
        }
	}
}
