using LiveryInstaller.UI.Models.DTO;

namespace LiveryInstaller.UI.Services;

/// <summary>
/// Represents a service that can provide aircraft configurations.
/// </summary>
public interface ILiveryConfigurationService
{
    /// <summary>
    /// Get available aircraft
    /// </summary>
    /// <returns>Available aircraft configurations. </returns>
    IReadOnlyCollection<AircraftDto> GetAvailableAircraft();
}