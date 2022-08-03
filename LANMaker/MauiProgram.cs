using LANMaker.Data;
using LANMaker.Services;
using Nito.AsyncEx;

namespace LANMaker;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
			});

		builder.Services.AddMauiBlazorWebView();
#if DEBUG
		builder.Services.AddBlazorWebViewDeveloperTools();
#endif

        builder.Services.AddBlazorWebView();


        builder.Services.AddSingleton<StartupService>();
        builder.Services.AddSingleton<StateContainer>();
        builder.Services.AddSingleton<ManifestService>();
        builder.Services.AddSingleton<ConfigurationService>();
        builder.Services.AddSingleton<InstallerService>();
        builder.Services.AddSingleton<GameRunService>();
        builder.Services.AddSingleton<DownloadTrackerService>();
        builder.Services.AddSingleton<CombinedGameService>();

        var host = builder.Build();

        AsyncContext.Run(() => Startup.RegisterLANMaker(host.Services));
        var settings = host.Services.GetRequiredService<StateContainer>();

        return host;
    }
}
