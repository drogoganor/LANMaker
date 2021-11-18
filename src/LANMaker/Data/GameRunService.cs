using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace LANMaker.Data
{
	public class GameRunService
	{
		private readonly ManifestService _manifestService;
		private Process process;

        public GameRunService(ManifestService manifestService)
        {
			_manifestService = manifestService;
        }

		public async Task PlayGame(ClientGame game)
		{
			if (process != null)
            {
				if (process.HasExited)
                {
					process = null;
                }
				else
                {
					// Game is already running; do nothing
					return;
                }
            }

			var installPath = Path.Combine(_manifestService.ConfigurationDirectory, game.Name, game.ExePath);

			if (!File.Exists(installPath))
            {
				throw new Exception($"Couldn't find path: {installPath}");
            }

			process = Process.Start(installPath);
		}
	}
}
