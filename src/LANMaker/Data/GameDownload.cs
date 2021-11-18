using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LANMaker.Data
{
    public enum GameDownloadStatus
    {
        Downloading,
        Installing,
    }

    public class GameDownload
    {
        public DateTime DownloadTime { get; set; }
        public TimeSpan Elapsed => DateTime.Now - DownloadTime;
        public ServerGame Game { get; set; }
        public GameDownloadStatus DownloadStatus { get; set; } = GameDownloadStatus.Downloading;
        public HttpClient HttpClient { get; set; }
        public CancellationToken CancellationToken { get; set; }

        public string ElapsedFormatted
        {
            get
            {
                var dt = new DateTime(Elapsed.Ticks);
                return dt.ToString("HH:mm:ss");
            }
        }
    }
}
