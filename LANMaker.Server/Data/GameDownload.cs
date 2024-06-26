﻿using LANMaker.Server.Enum;

namespace LANMaker.Data
{
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
