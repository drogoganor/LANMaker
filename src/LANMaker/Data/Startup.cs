using Microsoft.Maui.Controls.PlatformConfiguration;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using static System.Environment;

namespace LANMaker.Data
{
    public static class Startup
    {
        public static void CreateWorkingDirectory()
        {
            var appDataFolder = "LANMaker";
            var appDataPath = Path.Combine(Environment.GetFolderPath(SpecialFolder.MyDocuments), appDataFolder);

            if (!Directory.Exists(appDataPath))
            {
                try
                {
                    Directory.CreateDirectory(appDataPath);
                }
                catch
                {
                    throw;
                }
            }
        }
    }
}
