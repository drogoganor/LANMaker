using ElectronNET.API;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseElectron(args);

// Add services to the container.
builder.Services.AddElectron();

builder.Services.AddControllers();

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

//app.Run();

await app.StartAsync();

// Open the Electron-Window here
await Electron.WindowManager.CreateWindowAsync();

app.WaitForShutdown();