using LiveryInstaller.Library.Models.Configuration;
using LiveryInstaller.Library.Models.DTO;

namespace LiveryInstaller.Library.Services;

/// <summary>
/// Represents a service that can retrieve simulator information..
/// </summary>
public interface ISimulatorService
{
    /// <summary>
    /// Fetches the installed simualtors.
    /// </summary>
    /// <returns>A collection of <see cref="InstalledSimulator"/>.</returns>
    IReadOnlyList<InstalledSimulator> GetInstalledSimulators();

    /// <summary>
    /// Check if aircraft is installed.
    /// </summary>
    /// <param name="simulatorType">The desired simulator.</param>
    /// <param name="variantName">The name of the aircraft variant to check.</param>
    /// <returns>True if the aircraft is installed, otherwise false.</returns>
    bool IsAircraftInstalled(SimulatorType simulatorType, string variantName);

    /// <summary>
    /// Check if livery is installed.
    /// </summary>
    /// <param name="simulatorType">The desired simulator.</param>
    /// <param name="variantName">The name of the aircraft variant to check.</param>
    /// <param name="textureId">The texture ID of the livery to check.</param>
    /// <returns>True if the livery is installed, otherwise false.</returns>
    bool IsLiveryInstalled(SimulatorType simulatorType, string variantName, string textureId);

    /// <summary>
    /// Get livery path.
    /// </summary>
    /// <param name="simulatorType">The desired simulator.</param>
    /// <param name="variantName">The name of the aircraft variant.</param>
    /// <param name="textureId">The texture ID of the livery.</param>
    /// <returns>The path to the livery.</returns>
    string GetLiveryPath(SimulatorType simulatorType, string variantName, string textureId);

    /// <summary>
    /// Get variant configuration path.
    /// </summary>
    /// <param name="simulatorType">The desired simulator.</param>
    /// <param name="variantName">The name of the aircraft variant.</param>
    /// <returns>The path to the variant configuration file.</returns>
    string GetVariantConfigurationPath(SimulatorType simulatorType, string variantName);

    /// <summary>
    /// Get installation path.
    /// </summary>
    /// <param name="simulatorType">The desired simulator.</param>
    /// <returns>The path to the installation directory.</returns>
    string GetInstallationPath(SimulatorType simulatorType);
}