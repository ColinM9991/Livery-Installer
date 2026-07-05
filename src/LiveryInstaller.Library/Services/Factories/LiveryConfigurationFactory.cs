using LiveryInstaller.Library.Models.Configuration;
using LiveryInstaller.Library.Models.DTO;
using LiveryInstaller.Library.Services.Configuration;
using LiveryInstaller.Library.Services.Liveries;
using Microsoft.Extensions.Options;

namespace LiveryInstaller.Library.Services.Factories;

/// <inheritdoc />
public class LiveryConfigurationFactory(
    IReadableConfigurationStore<LiveriesConfiguration> readStore,
    IOptionsMonitor<AircraftConfiguration> aircraftConfiguration,
    IOptionsMonitor<UserSettings> userSettings,
    ILiveryPathProvider liveryPathProvider,
    ISimulatorService simulatorService)
    : ILiveryConfigurationFactory
{
    /// <inheritdoc />
    public async Task<IReadOnlyCollection<AircraftDto>> GetAvailableAircraftAsync(SimulatorType simulatorType)
    {
        if (string.IsNullOrWhiteSpace(userSettings.CurrentValue.LiveriesPath))
            return [];

        var userImportedLiveries = await readStore.ReadAsync();
        var merged = Merge(aircraftConfiguration.CurrentValue.Aircraft, userImportedLiveries.AircraftConfiguration.Aircraft);

        return merged
            .Where(x => liveryPathProvider.IsAircraftPathValid(x.Name))
            .Select(x => CreateAircraftDto(x, simulatorType))
            .Where(x => x.Variants.Count > 0)
            .ToList();
    }

    /// <summary>
    /// Creates a DTO for the given aircraft.
    /// </summary>
    /// <param name="aircraft">The aircraft to create a DTO for.</param>
    /// <returns>The created DTO.</returns>
    private AircraftDto CreateAircraftDto(Aircraft aircraft, SimulatorType simulatorType) => new(aircraft.Name,
        aircraft.Variants.Where(x =>
                liveryPathProvider.IsVariantPathValid(aircraft.Name, x.Name) &&
                simulatorService.IsAircraftInstalled(simulatorType, x.Name))
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

    private static List<Aircraft> Merge(ICollection<Aircraft> first, ICollection<Aircraft> second)
    {
        var copy = new List<Aircraft>(first);
        foreach (var aircraft in second)
        {
            var liveryConfigurationAircraft = copy.FirstOrDefault(x => x.Name == aircraft.Name);
            if (liveryConfigurationAircraft == null)
            {
                copy.Add(aircraft);
                liveryConfigurationAircraft = aircraft;
            }
                
            foreach (var variant in aircraft.Variants)
            {
                var liveryAircraftVariant = liveryConfigurationAircraft.Variants.FirstOrDefault(x => x.Name == variant.Name);
                if (liveryAircraftVariant == null)
                {
                    liveryConfigurationAircraft.Variants.Add(variant);
                    liveryAircraftVariant = variant;
                }
                    
                foreach (var livery in variant.Liveries)
                {
                    var liveryAircraftVariantLivery = liveryAircraftVariant.Liveries.FirstOrDefault(x => x.TextureId == livery.TextureId);
                    if (liveryAircraftVariantLivery == null)
                    {
                        liveryAircraftVariant.Liveries.Add(livery);
                    }
                }
            }
        }

        return copy;
    }
}