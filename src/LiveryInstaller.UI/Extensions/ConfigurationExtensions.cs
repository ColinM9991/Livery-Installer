using LiveryInstaller.UI.Models.Configuration;
using LiveryInstaller.UI.Services.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Win32;

namespace LiveryInstaller.UI.Extensions;

public static class ConfigurationExtensions
{
    public static HostApplicationBuilder UseAppConfiguration(this HostApplicationBuilder builder)
    {
        builder.Configuration
            .AddJsonFile(UserConfigurationStore.SettingsFile, optional: true, reloadOnChange: true)
            .AddJsonFile("liveryConfiguration.json", optional: false)
            .AddEnvironmentVariables();

        builder.Services.Configure<UserSettings>(
            builder.Configuration.GetSection("userSettings"));

        builder.Services.Configure<AircraftConfiguration>(builder.Configuration.GetSection("liveriesConfiguration"));

        builder.Services.PostConfigure<SimulatorConfiguration>(opts =>
        {
            var lockedSubKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Lockheed Martin\Prepar3D v5");
            opts.InstallationPath = lockedSubKey?.GetValue("SetupPath")
                as string;
        });

        return builder;
    }
}