using static System.Environment;

namespace LANMaker.Server
{
    public static class Paths
    {
        public static string ConfigDirectory => Path.Combine(GetFolderPath(SpecialFolder.MyDocuments), "LANMaker");
        public static string ConfigurationFile => Path.Combine(ConfigDirectory, "config.json");
        public static string ManifestPath => Path.Combine(ConfigDirectory, "manifest.json");
    }
}
