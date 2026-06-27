using LiveryInstaller.UI.Models.Configuration;
using LiveryInstaller.UI.Models.DTO;

namespace LiveryInstaller.UI.Services.Factories;

/// <summary>
/// Represents a service that can provide aircraft configurations.
/// </summary>
public interface ILiveryConfigurationFactory
{
    /// <summary>
    /// Get available aircraft
    /// </summary>
    /// <param name="simulatorType">The simulator to detect aircraft within.</param>
    /// <returns>Available aircraft configurations.</returns>
    Task<IReadOnlyCollection<AircraftDto>> GetAvailableAircraftAsync(SimulatorType simulatorType);
}