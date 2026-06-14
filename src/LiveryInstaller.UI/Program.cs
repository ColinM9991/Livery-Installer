using LiveryInstaller.UI.Models;
using LiveryInstaller.UI.Models.Configuration;
using LiveryInstaller.UI.Services;
using LiveryInstaller.UI.ViewModels;
using LiveryInstaller.UI.Views;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Win32;
using Serilog;
using Serilog.Core;
using Velopack;
using Velopack.Sources;

namespace LiveryInstaller.UI;

public static class Program
{
    private const string UpdateUrl = "https://github.com/ColinM9991/Livery-Installer";
    private static readonly string EnvironmentName =
#if DEBUG
        Environments.Development;
#else
        Environments.Production;
#endif

    [STAThread]
    private static void Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(new HostApplicationBuilderSettings
        {
            Args = args,
            EnvironmentName = EnvironmentName,
            DisableDefaults = false
        });

        builder.Logging.ClearProviders();

        builder.Configuration
            .AddJsonFile(SettingsStore.SettingsFile, optional: true, reloadOnChange: true)
            .AddJsonFile("liveryConfiguration.json", optional: false, reloadOnChange: false)
            .AddEnvironmentVariables();

        builder.Services.AddSerilog((services, logger) =>
        {
            var loggingLevelSwitch = services.GetRequiredService<LoggingLevelSwitch>();

            logger
                .ReadFrom
                .Configuration(builder.Configuration)
                .MinimumLevel.ControlledBy(loggingLevelSwitch);
        });

        ConfigureServices(builder.Services, builder.Configuration);

        var host = builder.Build();
        var services = host.Services;

        var userSettings = services.GetRequiredService<IOptionsMonitor<UserSettings>>();
        var loggingLevelSwitch = services.GetRequiredService<LoggingLevelSwitch>();

        userSettings.OnChange(x =>
        {
            loggingLevelSwitch.MinimumLevel = x.LogLevel.ToSerilogLevel();
        });

        VelopackApp.Build().Run();
        if (builder.Environment.IsProduction())
        {
            var applicationUpdater = services.GetRequiredService<IApplicationUpdaterService>();
            var updateInfo = applicationUpdater.CheckForUpdatesAsync().GetAwaiter().GetResult();
            if (updateInfo is not null)
            {
                applicationUpdater.ApplyUpdateAsync(updateInfo).GetAwaiter().GetResult();
            }
        }

        var app = new App(host);
        app.Run();
    }

    private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<MainWindow>();

        services.AddTransient<MainWindowViewModel>();
        services.AddTransient<LiveryPageViewModel>();
        services.AddTransient<SettingsPageViewModel>();
        services.AddSingleton<LoggingLevelSwitch>();

        services.AddSingleton<IApplicationUpdaterService, ApplicationUpdaterService>();
        services.AddSingleton(_ =>
            new UpdateManager(new GithubSource(UpdateUrl, null, false)));

        services.AddSingleton<IIconService, IconService>();
        services.AddSingleton<ISettingsStore, SettingsStore>();
        services.AddSingleton<INavigationService, NavigationService>();

        services.AddSingleton<ILiveryConfigurationService, LiveryConfigurationService>();
        services.AddSingleton<ISimulatorService, SimulatorService>();
        services.AddSingleton<IAvailableLiveryFactory, AvailableLiveryFactory>();
        services.AddSingleton<ILiveryViewModelFactory, LiveryViewModelFactory>();
        services.AddSingleton<ILiveryPathProvider, LiveryPathProvider>();
        services.AddSingleton<ILiveryInstallService, LiveryInstallService>();
        services.AddSingleton<ILiveryExtractor, LiveryExtractor>();
        services.AddSingleton<ITextureInstallService, TextureInstallService>();
        services.AddSingleton<IFileSystem, FileSystem>();

        services.AddSingleton<IAircraftConfigurationDeserializer, AircraftConfigurationDeserializer>();
        services.AddSingleton<IAircraftConfigurationSerializer, AircraftConfigurationSerializer>();

        services.AddSingleton<IAircraftConfigurationService, AircraftConfigurationService>();
        services.AddSingleton<IVariantConfigurationService, VariantConfigurationService>();

        services.Configure<UserSettings>(configuration.GetSection("userSettings"));
        services.Configure<AircraftConfiguration>(configuration.GetSection("liveriesConfiguration"));

        services.PostConfigure<SimulatorConfiguration>(opts =>
        {
            var lockedSubKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Lockheed Martin\Prepar3D v5");
            opts.InstallationPath = lockedSubKey?.GetValue("SetupPath")
                as string;
        });
    }
}