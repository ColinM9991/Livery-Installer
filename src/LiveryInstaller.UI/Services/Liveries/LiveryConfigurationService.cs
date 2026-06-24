using LiveryInstaller.UI.Models.Configuration;
using LiveryInstaller.UI.Models.DTO;
using LiveryInstaller.UI.Services.Configuration;

namespace LiveryInstaller.UI.Services.Liveries;

public sealed class LiveryConfigurationService(
    IReadableConfigurationStore<LiveriesConfiguration> readStore,
    IWriteableConfigurationStore<LiveriesConfiguration> writeStore) : ILiveryConfigurationService
{
    public async Task<bool> IsLiveryInstalledAsync(string aircraftName, string variantName, string liveryTextureId)
    {
        var configuration = await readStore.ReadAsync();
        var variant = GetVariant(configuration, aircraftName, variantName);

        var existingLivery = variant.Liveries.FirstOrDefault(x => x.TextureId == liveryTextureId);

        return existingLivery is not null;
    }

    public async Task InstallLiveryAsync(string aircraftName, string variantName, LiveryDto livery)
    {
        var configuration = await readStore.ReadAsync();
        var variant = GetVariant(configuration, aircraftName, variantName);

        var existingLivery = variant.Liveries.FirstOrDefault(x => x.TextureId == livery.TextureId);
        if (existingLivery is not null)
        {
            throw new InvalidOperationException(
                $"Livery {livery.TextureId} already exists for variant {variantName} and aircraft {aircraftName}");
        }

        variant.Liveries.Add(new Livery(livery));
        await writeStore.WriteAsync(configuration);
    }

    private static Variant GetVariant(LiveriesConfiguration liveriesConfiguration, string aircraftName,
        string variantName)
    {
        var configuration = liveriesConfiguration.AircraftConfiguration ??= new AircraftConfiguration();
        configuration.Aircraft ??= new List<Aircraft>();

        var aircraft = configuration.Aircraft.FirstOrDefault(x => x.Name == aircraftName);
        if (aircraft is null)
        {
            aircraft = new Aircraft { Name = aircraftName };
            configuration.Aircraft.Add(aircraft);
        }

        var variant = aircraft.Variants.FirstOrDefault(x => x.Name == variantName);
        if (variant is null)
        {
            variant = new Variant { Name = variantName };
            aircraft.Variants.Add(variant);
        }

        return variant;
    }
}