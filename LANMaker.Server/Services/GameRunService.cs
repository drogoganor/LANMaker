using System.Diagnostics;
using LANMaker.Data;
using LANMaker.Server;

namespace LANMaker.Services
{
    public class GameRunService
    {
        private Process process;
        private readonly StateContainer stateContainer;

        public GameRunService(StateContainer stateContainer)
        {
            this.stateContainer = stateContainer;
        }

        public async Task PlayGame(string name)
        {
            var game = stateContainer.Configuration.InstalledGames.FirstOrDefault(x => x.Name == name);

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

            var workingDirectory = Path.Combine(Paths.ConfigDirectory, game.Name);
            var processPath = Path.Combine(workingDirectory, game.ExePath);

            if (!File.Exists(processPath))
            {
                throw new Exception($"Couldn't find path: {processPath}");
            }

            var processInfo = new ProcessStartInfo
            {
                FileName = processPath,
                WorkingDirectory = workingDirectory,
            };

            process = Process.Start(processInfo);
        }
    }
}
