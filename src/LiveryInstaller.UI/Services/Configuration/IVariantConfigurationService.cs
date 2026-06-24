namespace LiveryInstaller.UI.Services.Configuration;

/// <summary>
/// Represents a service that can manage variant configurations.
/// </summary>
public interface IVariantConfigurationService
{
    /// <summary>
    /// Installs aircraft variant configuration.
    /// </summary>
    /// <param name="sourceArchiveDirectory">The path to the source archive directory.</param>
    /// <param name="aircraftName">The name of the aircraft.</param>
    /// <param name="atcId">The ATC ID of the aircraft.</param>
    void InstallVariantConfiguration(string sourceArchiveDirectory, string aircraftName, string atcId);

    /// <summary>
    /// Uninstalls aircraft variant configuration.
    /// </summary>
    /// <param name="aircraftName">The name of the aircraft.</param>
    /// <param name="atcId">The ATC ID of the aircraft.</param>
    void UninstallVariantConfiguration(string aircraftName, string atcId);
}