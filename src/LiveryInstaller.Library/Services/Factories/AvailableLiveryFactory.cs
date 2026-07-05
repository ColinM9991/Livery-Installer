using LiveryInstaller.Library.Models.Configuration;
using LiveryInstaller.Library.Models.DTO;
using LiveryInstaller.Library.Services.Liveries;

namespace LiveryInstaller.Library.Services.Factories;

/// <inheritdoc />
public sealed class AvailableLiveryFactory(
    ISimulatorService simulatorService,
    ILiveryPathProvider liveryPathProvider): IAvailableLiveryFactory
{
    /// <inheritdoc />
    public AvailableLivery Create(SimulatorType simulatorType, AircraftDto aircraft, VariantDto variant, LiveryDto livery)
    {
        ArgumentNullException.ThrowIfNull(aircraft);
        ArgumentNullException.ThrowIfNull(variant);
        ArgumentNullException.ThrowIfNull(livery);

        var availableLivery = new AvailableLivery(aircraft, variant, livery)
        {
            IsInstalled = simulatorService.IsLiveryInstalled(simulatorType, variant.Name, livery.TextureId),
            IconPath = liveryPathProvider.GetIconPath(aircraft.Name, variant.Name, livery.SanitisedName),
            LiveryPath = liveryPathProvider.GetLiveryPath(aircraft.Name, variant.Name, livery.SanitisedName)
        };

        return availableLivery;
    }
}