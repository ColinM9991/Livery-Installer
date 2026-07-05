using LiveryInstaller.Library.Models.Configuration;
using AircraftConfiguration = LiveryInstaller.Library.Models.INI.AircraftConfiguration;

namespace LiveryInstaller.Library.Services.Configuration;

/// <summary>
/// Represents a service that can manage aircraft configurations.
/// </summary>
[LoggingDecorator]
public interface IAircraftConfigurationService
{
    /// <summary>
    /// Adds livery to variant configuration
    /// </summary>
    /// <param name="simulatorType">The simulator the livery is being added to.</param>
    /// <param name="variantName">The name of the variant to which the livery will be added.</param>
    /// <param name="archiveDirectory">The directory path of the livery archive.</param>
    /// <returns>A Task that represents the asynchronous operation.</returns>
    Task AddLiveryAsync(SimulatorType simulatorType, string variantName, string archiveDirectory);

    /// <summary>
    /// Removes livery from variant configuration
    /// </summary>
    /// <param name="simulatorType">The simulator the livery is being removed from.</param>
    /// <param name="variantName">The name of the variant from which to remove the livery.</param>
    /// <param name="atcId">The ATC ID of the livery to remove.</param>
    /// <returns>A Task that represents the asynchronous operation.</returns>
    Task RemoveLiveryAsync(SimulatorType simulatorType, string variantName, string atcId);

    Task<AircraftConfiguration> LoadAircraftConfigurationAsync(string configurationFile);
}