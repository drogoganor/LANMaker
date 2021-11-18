using Microsoft.AspNetCore.Components.WebView.Maui;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui;
using Microsoft.Maui.Hosting;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls.Hosting;
using LANMaker.Data;
using System;

namespace LANMaker
{
	public static class MauiProgram
	{
		public static MauiApp CreateMauiApp()
		{
			var builder = MauiApp.CreateBuilder();
			builder
				.RegisterBlazorMauiWebView()
				.UseMauiApp<App>()
				.ConfigureFonts(fonts =>
				{
					fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				});

			builder.Services.AddBlazorWebView();
			builder.Services.AddSingleton<ManifestService>();
			builder.Services.AddSingleton<ConfigurationService>();
			builder.Services.AddSingleton<InstallerService>();
			builder.Services.AddSingleton<GameRunService>();
			builder.Services.AddSingleton<DownloadTrackerService>();

			return builder.Build();
		}
	}
}