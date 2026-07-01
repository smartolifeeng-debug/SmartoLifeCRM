using System.IO;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SmartoLifeCRM.App.Data;
using SmartoLifeCRM.App.Services;
using SmartoLifeCRM.App.ViewModels;

namespace SmartoLifeCRM.App;

public partial class App : Application
{
    private readonly IHost _host;

    public App()
    {
        _host = Host.CreateDefaultBuilder()
            .ConfigureAppConfiguration((_, config) =>
            {
                config.SetBasePath(AppContext.BaseDirectory);
                config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            })
            .ConfigureServices((context, services) =>
            {
                services.AddSingleton<IAppPathService, AppPathService>();
                services.AddSingleton<IConfigurationService, ConfigurationService>();
                services.AddSingleton<IThemeService, ThemeService>();
                services.AddSingleton<ILoggingService, FileLoggingService>();
                services.AddSingleton<IErrorHandler, ErrorHandler>();
                services.AddDbContext<SmartoLifeDbContext>((provider, options) =>
                {
                    var pathService = provider.GetRequiredService<IAppPathService>();
                    var configuredConnection = context.Configuration.GetConnectionString("DefaultConnection");
                    var connectionString = string.IsNullOrWhiteSpace(configuredConnection)
                        ? $"Data Source={pathService.DatabasePath}"
                        : configuredConnection.Replace("{AppData}", pathService.AppDataDirectory);

                    options.UseSqlite(connectionString);
                });

                services.AddScoped<IDatabaseInitializer, DatabaseInitializer>();
                services.AddSingleton<DashboardViewModel>();
                services.AddSingleton<SettingsPageViewModel>();
                services.AddSingleton<INavigationService, NavigationService>();
                services.AddSingleton<MainViewModel>();
                services.AddSingleton<MainWindow>();
            })
            .Build();
    }

    protected override async void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        try
        {
            var pathService = _host.Services.GetRequiredService<IAppPathService>();
            Directory.CreateDirectory(pathService.AppDataDirectory);
            Directory.CreateDirectory(pathService.LogsDirectory);
            Directory.CreateDirectory(pathService.SettingsDirectory);

            _host.Services.GetRequiredService<ILoggingService>().Info("Starting Smarto Life CRM v0.3.0 shell.");

            using (var scope = _host.Services.CreateScope())
            {
                var databaseInitializer = scope.ServiceProvider.GetRequiredService<IDatabaseInitializer>();
                await databaseInitializer.InitializeAsync();
            }

            var mainWindow = _host.Services.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }
        catch (Exception exception)
        {
            _host.Services.GetRequiredService<IErrorHandler>().Handle(
                exception,
                "Smarto Life CRM could not start. Please check the local log file for details.");
            Shutdown(1);
        }
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        await _host.StopAsync(TimeSpan.FromSeconds(5));
        _host.Dispose();
        base.OnExit(e);
    }
}

