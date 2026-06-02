using LiveryInstaller.UI.Models.DTO;
using LiveryInstaller.UI.ViewModels;

namespace LiveryInstaller.UI.Services;

public class LiveryViewModelFactory(
    IAvailableLiveryFactory availableLiveryFactory,
    ILiveryInstallService liveryInstallService,
    IIconService iconService)
    : ILiveryViewModelFactory
{
    public LiveryViewModel Create(AircraftDto aircraft, VariantDto variant, LiveryDto livery)
    {
        ArgumentNullException.ThrowIfNull(aircraft);
        ArgumentNullException.ThrowIfNull(variant);
        ArgumentNullException.ThrowIfNull(livery);

        var availableLivery = availableLiveryFactory.Create(aircraft, variant, livery);
        return new LiveryViewModel(availableLivery, liveryInstallService, iconService);
    }
}