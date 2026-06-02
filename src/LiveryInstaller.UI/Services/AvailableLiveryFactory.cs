using LiveryInstaller.UI.Models.DTO;

namespace LiveryInstaller.UI.Services;

/// <inheritdoc />
public sealed class AvailableLiveryFactory(
    ISimulatorService simulatorService,
    ILiveryPathProvider liveryPathProvider): IAvailableLiveryFactory
{
    /// <inheritdoc />
    public AvailableLivery Create(AircraftDto aircraft, VariantDto variant, LiveryDto livery)
    {
        ArgumentNullException.ThrowIfNull(aircraft);
        ArgumentNullException.ThrowIfNull(variant);
        ArgumentNullException.ThrowIfNull(livery);

        var availableLivery = new AvailableLivery(aircraft, variant, livery)
        {
            IsInstalled = simulatorService.IsLiveryInstalled(variant.Name, livery.TextureId),
            IconPath = liveryPathProvider.GetIconPath(aircraft.Name, variant.Name, livery.SanitisedName),
            LiveryPath = liveryPathProvider.GetLiveryPath(aircraft.Name, variant.Name, livery.SanitisedName)
        };

        return availableLivery;
    }
}