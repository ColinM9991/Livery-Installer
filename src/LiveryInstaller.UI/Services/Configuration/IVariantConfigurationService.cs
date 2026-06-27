using LiveryInstaller.UI.Models.Configuration;

namespace LiveryInstaller.UI.Services.Configuration;

/// <summary>
/// Represents a service that can manage variant configurations.
/// </summary>
[LoggingDecorator]
public interface IVariantConfigurationService
{
    /// <summary>
    /// Installs aircraft variant configuration.
    /// </summary>
    /// <param name="simulatorType">The simulator the livery configuration is being added to.</param>
    /// <param name="sourceArchiveDirectory">The path to the source archive directory.</param>
    /// <param name="aircraftName">The name of the aircraft.</param>
    /// <param name="atcId">The ATC ID of the aircraft.</param>
    void InstallVariantConfiguration(SimulatorType simulatorType, string sourceArchiveDirectory, string aircraftName, string atcId);

    /// <summary>
    /// Uninstalls aircraft variant configuration.
    /// </summary>
    /// <param name="simulatorType">The simulator the livery configuration is being removed from.</param>
    /// <param name="aircraftName">The name of the aircraft.</param>
    /// <param name="atcId">The ATC ID of the aircraft.</param>
    void UninstallVariantConfiguration(SimulatorType simulatorType, string aircraftName, string atcId);
}