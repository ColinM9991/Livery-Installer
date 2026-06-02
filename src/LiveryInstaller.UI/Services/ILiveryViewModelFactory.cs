using LiveryInstaller.UI.Models.DTO;
using LiveryInstaller.UI.ViewModels;

namespace LiveryInstaller.UI.Services;

/// <summary>
/// Represents a service that can create a <see cref="LiveryViewModel"/> from a <see cref="AircraftDto"/>, <see cref="VariantDto"/> and <see cref="LiveryDto"/>.
/// </summary>
public interface ILiveryViewModelFactory
{
    /// <summary>
    /// Creates a <see cref="LiveryViewModel"/> from a <see cref="AircraftDto"/>, <see cref="VariantDto"/> and <see cref="LiveryDto"/>.
    /// </summary>
    /// <param name="aircraft">The aircraft to create the view model for. </param>
    /// <param name="variant">The variant to create the view model for. </param>
    /// <param name="livery">The livery to create the view model for. </param>
    /// <returns>The created <see cref="LiveryViewModel"/>. </returns>
    LiveryViewModel Create(AircraftDto aircraft, VariantDto variant, LiveryDto livery);
}