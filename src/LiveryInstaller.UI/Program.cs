using System.IO;
using LiveryInstaller.UI.Extensions;
using LiveryInstaller.UI.Helpers;
using LiveryInstaller.UI.Services.Configuration;
using Microsoft.Extensions.Hosting;

namespace LiveryInstaller.UI;

public static class Program
{
    private static readonly string EnvironmentName =
#if DEBUG
        Environments.Development;
#else
        Environments.Production;
#endif

    [STAThread]
    private static void Main(string[] args)
    {
        EnsureApplicationConfiguration();

        var builderSettings = new HostApplicationBuilderSettings
        {
            Args = args,
            EnvironmentName = EnvironmentName,
            DisableDefaults = false
        };

        var host = Host.CreateApplicationBuilder(builderSettings)
            .UseAppConfiguration()
            .UseAppLogging()
            .ConfigureAppServices()
            .Build();
        
        host.InitializeRuntimeServices();
        host.RunApp();
    }

    /// <summary>
    /// Creates the user configuration space to ensure reloadOnChange works for first time runs.
    /// </summary>
    private static void EnsureApplicationConfiguration()
    {
        if (!Directory.Exists(Paths.SettingsDirectory))
        {
            Directory.CreateDirectory(Paths.SettingsDirectory);
        }

        EnsureConfigFile(LiveriesConfigurationStore.ConfigurationFile);
        EnsureConfigFile(UserConfigurationStore.SettingsFile);
    }

    /// <summary>
    /// Creates the initial configuration file if it does not exist.
    /// </summary>
    /// <param name="path">The path to the configuration file</param>
    /// <exception cref="ArgumentException">If the path is null or whitespace</exception>
    private static void EnsureConfigFile(string path)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(path);

        if (!File.Exists(path))
        {
            File.WriteAllText(path, "{}");
        }
    }
}