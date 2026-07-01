using LiveryInstaller.UI.Models.Configuration;
using LiveryInstaller.UI.Models.DTO;
using LiveryInstaller.UI.Services.Icons;
using LiveryInstaller.UI.Services.Liveries;
using LiveryInstaller.UI.ViewModels;

namespace LiveryInstaller.UI.Services.Factories;

public class LiveryViewModelFactory(
    IAvailableLiveryFactory availableLiveryFactory,
    ILiveryInstallService liveryInstallService,
    ILiveryImportService liveryImportService,
    IIconService iconService)
    : ILiveryViewModelFactory
{
    public LiveryViewModel Create(SimulatorType simulatorType, AircraftDto aircraft, VariantDto variant, LiveryDto livery, Action<LiveryViewModel> onDeleteCallback)
    {
        ArgumentNullException.ThrowIfNull(aircraft);
        ArgumentNullException.ThrowIfNull(variant);
        ArgumentNullException.ThrowIfNull(livery);

        var availableLivery = availableLiveryFactory.Create(simulatorType, aircraft, variant, livery);
        return new LiveryViewModel(simulatorType, availableLivery, liveryInstallService, liveryImportService, iconService, onDeleteCallback);
    }
}