using LiveryInstaller.UI.Models.Configuration;
using LiveryInstaller.UI.Models.DTO;

namespace LiveryInstaller.UI.Services.Factories;

/// <summary>
/// Represents a factory that can create <see cref="AvailableLivery"/> instances.
/// </summary>
public interface IAvailableLiveryFactory
{
    /// <summary>
    /// Creates a new instance of <see cref="AvailableLivery"/>.
    /// </summary>
    /// <param name="simulatorType">The selected simulator.</param>
    /// <param name="aircraft">The aircraft to create the livery for.</param>
    /// <param name="variant">The variant to create the livery for.</param>
    /// <param name="livery">The livery to create.</param>
    /// <returns></returns>
    AvailableLivery Create(SimulatorType simulatorType, AircraftDto aircraft, VariantDto variant, LiveryDto livery);
}