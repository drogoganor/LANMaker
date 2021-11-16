using System;

namespace LANMaker.Data
{
	/// <summary>
	/// Client configuration. 
	/// </summary>
	public class Configuration
	{
		public string ManifestUrl { get; set; }
		public string StoragePath { get; set; }
		public ClientGame[] InstalledGames { get; set; }
	}
}
