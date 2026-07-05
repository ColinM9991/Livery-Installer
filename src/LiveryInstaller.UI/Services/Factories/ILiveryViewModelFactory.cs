using LiveryInstaller.Library.Models.Configuration;
using LiveryInstaller.Library.Models.DTO;
using LiveryInstaller.UI.ViewModels;

namespace LiveryInstaller.UI.Services.Factories;

/// <summary>
/// Represents a service that can create a <see cref="LiveryViewModel"/> from a <see cref="AircraftDto"/>, <see cref="VariantDto"/> and <see cref="LiveryDto"/>.
/// </summary>
public interface ILiveryViewModelFactory
{
    /// <summary>
    /// Creates a <see cref="LiveryViewModel"/> from a <see cref="AircraftDto"/>, <see cref="VariantDto"/> and <see cref="LiveryDto"/>.
    /// </summary>
    /// <param name="simulatorType">The desired simulator.</param>
    /// <param name="aircraft">The aircraft to create the view model for. </param>
    /// <param name="variant">The variant to create the view model for. </param>
    /// <param name="livery">The livery to create the view model for. </param>
    /// <param name="onDeleteCallback">The callback to be called when the livery is deleted. </param>
    /// <returns>The created <see cref="LiveryViewModel"/>. </returns>
    LiveryViewModel Create(SimulatorType simulatorType, AircraftDto aircraft, VariantDto variant, LiveryDto livery, Action<LiveryViewModel> onDeleteCallback);
}