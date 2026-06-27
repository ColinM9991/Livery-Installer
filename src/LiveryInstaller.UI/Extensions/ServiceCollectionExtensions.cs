using CommunityToolkit.Mvvm.Messaging;
using LiveryInstaller.UI.Models.Configuration;
using LiveryInstaller.UI.Services;
using LiveryInstaller.UI.Services.Configuration;
using LiveryInstaller.UI.Services.Factories;
using LiveryInstaller.UI.Services.Icons;
using LiveryInstaller.UI.Services.Liveries;
using LiveryInstaller.UI.Services.Parsing;
using LiveryInstaller.UI.ViewModels;
using LiveryInstaller.UI.Views;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog.Core;
using Velopack;
using Velopack.Sources;

namespace LiveryInstaller.UI.Extensions;

public static class ServiceCollectionExtensions
{
    public static HostApplicationBuilder ConfigureAppServices(this HostApplicationBuilder builder)
    {
        var services = builder.Services;

        services.AddScoped<MainWindow>();
        services.AddScoped<MainWindowViewModel>();
        services.AddScoped<LiveryPageViewModel>();
        services.AddScoped<ImportLiveryPageViewModel>();
        services.AddScoped<SettingsPageViewModel>();
        services.AddSingleton<ToastControlViewModel>();

        services.AddSingleton<INavigationService, NavigationService>();
        services.AddSingleton<IToastService, ToastService>();
        services.AddSingleton<IMessenger, WeakReferenceMessenger>();
        services.AddSingleton<IApplicationUpdaterService, ApplicationUpdaterService>();
        services.AddSingleton<ISimulatorService, SimulatorService>();

        services.AddSingleton<IFileSystem, FileSystem>();
        services.AddSingleton<LoggingLevelSwitch>();

        RegisterLiveryServices(services);
        RegisterConfigurationServices(services);
        RegisterParsingServices(services);
        RegisterFactoryServices(services);
        RegisterIconServices(services);
        RegisterUpdaterServices(services, builder.Configuration);

        return builder;
    }

    private static void RegisterLiveryServices(IServiceCollection services)
    {
        services.AddSingleton<ILiveryInstallService, LiveryInstallService>()
            .Decorate<ILiveryInstallService, LoggingLiveryInstallService>()
            .Decorate<ILiveryInstallService, NotifyingLiveryInstallService>();

        services.AddSingleton<ILiveryImportService, LiveryImportService>()
            .Decorate<ILiveryImportService, LoggingLiveryImportService>()
            .Decorate<ILiveryImportService, NotifyingLiveryImportService>();
        
        services.AddSingleton<ITextureInstallService, TextureInstallService>();
        services.AddSingleton<ILiveryPathProvider, LiveryPathProvider>();
        services.AddSingleton<ILiveryExtractor, LiveryExtractor>();
    }

    private static void RegisterFactoryServices(IServiceCollection services)
    {
        services.AddSingleton<ILiveryConfigurationFactory, LiveryConfigurationFactory>();
        services.AddSingleton<IAvailableLiveryFactory, AvailableLiveryFactory>();
        services.AddSingleton<ILiveryViewModelFactory, LiveryViewModelFactory>();
        services.AddSingleton<ILoadedLiveryFactory, LoadedLiveryFactory>();
    }

    private static void RegisterConfigurationServices(IServiceCollection services)
    {
        services.AddSingleton<IWriteableConfigurationStore<UserSettings>, UserConfigurationStore>();
        services.AddSingleton<IReadableConfigurationStore<LiveriesConfiguration>, LiveriesConfigurationStore>();
        services.AddSingleton<IWriteableConfigurationStore<LiveriesConfiguration>, LiveriesConfigurationStore>();
        services.AddSingleton<ILiveryConfigurationService, LiveryConfigurationService>();
        services.AddSingleton<IAircraftConfigurationService, AircraftConfigurationService>();
        services.AddSingleton<IVariantConfigurationService, VariantConfigurationService>();
    }

    private static void RegisterIconServices(IServiceCollection services)
    {
        services
            .AddSingleton<IIconService, IconService>()
            .Decorate<IIconService, CachingIconService>();
    }

    private static void RegisterParsingServices(IServiceCollection services)
    {
        services.AddSingleton<IAircraftConfigurationParser, AircraftConfigurationParser>();
        services.AddSingleton<IIniLexer, IniLexer>();
        services.AddSingleton<IAircraftConfigurationDeserializer, AircraftConfigurationDeserializer>();
        services.AddSingleton<IAircraftConfigurationSerializer, AircraftConfigurationSerializer>();
    }

    private static void RegisterUpdaterServices(IServiceCollection services, IConfigurationManager config)
    {
        const string updateUrl = "https://github.com/ColinM9991/Livery-Installer";
        services.AddSingleton(_ =>
            new UpdateManager(new GithubSource(
                config["UpdateUrl"] ?? updateUrl,
                null,
                false)));
    }
}