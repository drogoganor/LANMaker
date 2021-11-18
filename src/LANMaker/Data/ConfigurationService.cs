using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using static System.Environment;

namespace LANMaker.Data
{
	public class ConfigurationService
	{
		private string configPath => Path.Combine(Directory.GetParent(AppContext.BaseDirectory).FullName, "Resources/config.json");

        public ConfigurationService()
        {
        }

		public async Task<Configuration> GetConfiguration()
		{
			try
            {
				using (var stream = new FileStream(configPath, FileMode.Open, FileAccess.Read))
				{
					var configuration = await JsonSerializer.DeserializeAsync<Configuration>(stream);
					return configuration;
				}
            }
			catch
            {
				throw;
			}
		}

		public async Task SaveConfiguration(Configuration configuration)
		{
			try
			{
				var json = JsonSerializer.Serialize(configuration, new JsonSerializerOptions
                {
					AllowTrailingCommas = true,
					WriteIndented = true,
                });

                using var configFile = new StreamWriter(configPath);
                await configFile.WriteAsync(json);
            }
			catch
			{
				throw;
			}
		}
	}
}
