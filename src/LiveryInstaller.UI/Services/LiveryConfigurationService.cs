using LiveryInstaller.UI.Models.Configuration;
using LiveryInstaller.UI.Models.DTO;
using Microsoft.Extensions.Options;

namespace LiveryInstaller.UI.Services;

/// <inheritdoc />
public class LiveryConfigurationService(
    IOptions<AircraftConfiguration> aircraftConfiguration,
    IOptionsMonitor<UserSettings> userSettings,
    ILiveryPathProvider liveryPathProvider,
    ISimulatorService simulatorService)
    : ILiveryConfigurationService
{
    /// <inheritdoc />
    public IReadOnlyCollection<AircraftDto> GetAvailableAircraft()
    {
        if (string.IsNullOrWhiteSpace(userSettings.CurrentValue.LiveriesPath))
            return [];
        
        return aircraftConfiguration.Value.Aircraft
            .Where(x => liveryPathProvider.IsLiveryPathValid(x.Name))
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
        aircraft.Variants.Where(x => simulatorService.IsAircraftInstalled(x.Name))
            .Select(CreateVariantDto).ToList());

    /// <summary>
    /// Creates a DTO for the given variant.
    /// </summary>
    /// <param name="variant">The variant to create a DTO for.</param>
    /// <returns>The created DTO.</returns>
    private static VariantDto CreateVariantDto(Variant variant) =>
        new(variant.Name, variant.Liveries.Select(CreateLiveryDto).ToList());

    /// <summary>
    /// Creates a DTO for the given livery.
    /// </summary>
    /// <param name="livery">The livery to create a DTO for.</param>
    /// <returns>The created DTO.</returns>
    private static LiveryDto CreateLiveryDto(Livery livery) => new(livery.TextureId, livery.AtcId,
        livery.Name,
        livery.Description, livery.Airline, livery.SanitisedName);
}