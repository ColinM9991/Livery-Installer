using LiveryInstaller.Library.Models.Configuration;
using LiveryInstaller.Library.Models.DTO;

namespace LiveryInstaller.Library.Services.Factories;

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