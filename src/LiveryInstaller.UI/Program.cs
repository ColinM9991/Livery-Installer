using LiveryInstaller.UI.Models.Configuration;
using LiveryInstaller.UI.Services;
using LiveryInstaller.UI.ViewModels;
using LiveryInstaller.UI.Views;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Win32;

namespace LiveryInstaller.UI;

public static class Program
{
    [STAThread]
    private static void Main(string[] args)
    {
        var host = Host.CreateDefaultBuilder()
            .ConfigureLogging(builder => { builder.SetMinimumLevel(LogLevel.Information).AddConsole(); })
            .ConfigureAppConfiguration(builder =>
            {
                builder
                    .AddJsonFile(SettingsStore.SettingsFile, optional: true, reloadOnChange: true)
                    .AddJsonFile("liveryConfiguration.json", optional: false, reloadOnChange: false)
                    .AddEnvironmentVariables();
            })
            .ConfigureServices(ConfigureServices)
            .Build();

        var serviceProvider = host.Services;

        var app = new App();
        var mainWindow = serviceProvider.GetRequiredService<MainWindow>();

        app.SetStartupWindow(mainWindow);
        app.InitializeComponent();
        app.Run();
    }

    private static void ConfigureServices(HostBuilderContext context, IServiceCollection services)
    {
        var configuration = context.Configuration;

        services.AddScoped<MainWindow>();

        services.AddSingleton<MainWindowViewModel>();
        services.AddSingleton<LiveryPageViewModel>();
        services.AddSingleton<SettingsPageViewModel>();

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