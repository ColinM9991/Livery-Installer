namespace LiveryInstaller.UI.Services;

/// <summary>
/// Represents a service that can retrieve simulator information..
/// </summary>
public interface ISimulatorService
{
    /// <summary>
    /// Check if aircraft is installed.
    /// </summary>
    /// <param name="variantName">The name of the aircraft variant to check.</param>
    /// <returns>True if the aircraft is installed, otherwise false.</returns>
    bool IsAircraftInstalled(string variantName);
    
    /// <summary>
    /// Check if livery is installed.
    /// </summary>
    /// <param name="variantName">The name of the aircraft variant to check.</param>
    /// <param name="textureId">The texture ID of the livery to check.</param>
    /// <returns>True if the livery is installed, otherwise false.</returns>
    bool IsLiveryInstalled(string variantName, string textureId);
    
    /// <summary>
    /// Get livery path.
    /// </summary>
    /// <param name="variantName">The name of the aircraft variant.</param>
    /// <param name="textureId">The texture ID of the livery.</param>
    /// <returns>The path to the livery.</returns>
    string GetLiveryPath(string variantName, string textureId);

    /// <summary>
    /// Get variant configuration path.
    /// </summary>
    /// <param name="variantName">The name of the aircraft variant.</param>
    /// <returns>The path to the variant configuration file.</returns>
    string GetVariantConfigurationPath(string variantName);

    /// <summary>
    /// Get installation path.
    /// </summary>
    /// <returns>The path to the installation directory.</returns>
    string GetInstallationPath();
}