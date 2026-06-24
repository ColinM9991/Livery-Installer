using LiveryInstaller.UI.Models.Configuration;
using LiveryInstaller.UI.Models.DTO;
using LiveryInstaller.UI.Services.Factories;
using Microsoft.Extensions.Options;

namespace LiveryInstaller.UI.Services.Liveries;

/// <inheritdoc />
public class AircraftDtoFactory(
    IOptionsMonitor<AircraftConfiguration> aircraftConfiguration,
    IOptionsMonitor<UserSettings> userSettings,
    ILiveryPathProvider liveryPathProvider,
    ISimulatorService simulatorService)
    : ILiveryConfigurationFactory
{
    /// <inheritdoc />
    public IReadOnlyCollection<AircraftDto> GetAvailableAircraft()
    {
        if (string.IsNullOrWhiteSpace(userSettings.CurrentValue.LiveriesPath))
            return [];

        return aircraftConfiguration.CurrentValue.Aircraft
            .Where(x => liveryPathProvider.IsAircraftPathValid(x.Name))
            .Select(CreateAircraftDto)
            .Where(x => x.Variants.Count > 0)
            .ToList();
    }

    /// <summary>
    /// Creates a DTO for the given aircraft.
    /// </summary>
    /// <param name="aircraft">The aircraft to create a DTO for.</param>
    /// <returns>The created DTO.</returns>
    private AircraftDto CreateAircraftDto(Aircraft aircraft) => new(aircraft.Name,
        aircraft.Variants.Where(x =>
                liveryPathProvider.IsVariantPathValid(aircraft.Name, x.Name) &&
                simulatorService.IsAircraftInstalled(x.Name))
            .Select(x => CreateVariantDto(aircraft, x))
            .Where(x => x.Liveries.Count > 0).ToList());

    /// <summary>
    /// Creates a DTO for the given variant.
    /// </summary>
    /// <param name="aircraft">The aircraft the variant belongs to.</param>
    /// <param name="variant">The variant to create a DTO for.</param>
    /// <returns>The created DTO.</returns>
    private VariantDto CreateVariantDto(Aircraft aircraft, Variant variant) =>
        new(variant.Name,
            variant.Liveries.Where(x =>
                    liveryPathProvider.IsLiveryPathValid(aircraft.Name, variant.Name, x.SanitisedName))
                .Select(CreateLiveryDto).ToList());

    /// <summary>
    /// Creates a DTO for the given livery.
    /// </summary>
    /// <param name="livery">The livery to create a DTO for.</param>
    /// <returns>The created DTO.</returns>
    private static LiveryDto CreateLiveryDto(Livery livery) => new(livery);
}