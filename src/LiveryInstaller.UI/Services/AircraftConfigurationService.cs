using System.IO;
using LiveryInstaller.UI.Models.INI;
using Microsoft.Extensions.Logging;

namespace LiveryInstaller.UI.Services;

/// <inheritdoc />
public sealed class AircraftConfigurationService(
    ISimulatorService simulatorService,
    IAircraftConfigurationDeserializer aircraftConfigurationDeserializer,
    IAircraftConfigurationSerializer aircraftConfigurationSerializer,
    ILogger<AircraftConfigurationService> logger) : IAircraftConfigurationService
{
    /// <inheritdoc />
    public async Task AddLiveryAsync(string variantName, string archiveDirectory)
    {
        logger.LogInformation("Adding livery for variant {VariantName}", variantName);

        var aircraftConfigPath = simulatorService.GetVariantConfigurationPath(variantName);

        var tempConfigPath = Path.Combine(archiveDirectory, "Config.cfg");

        AircraftConfiguration deserializedAircraftConfig;
        using (var tempConfig = new StreamReader(tempConfigPath))
        using (var aircraftConfig = new StreamReader(aircraftConfigPath))
        {
            var deserializedTempConfig = await aircraftConfigurationDeserializer.DeserializeAsync(tempConfig);

            deserializedAircraftConfig =
                await aircraftConfigurationDeserializer.DeserializeAsync(aircraftConfig);

            deserializedAircraftConfig.Merge(deserializedTempConfig);
        }

        await using (var aircraftConfigStream = new StreamWriter(aircraftConfigPath))
        {
            await aircraftConfigurationSerializer.SerializeAsync(deserializedAircraftConfig,
                aircraftConfigStream);
        }
    }

    /// <inheritdoc />
    public async Task RemoveLiveryAsync(string variantName, string atcId)
    {
        logger.LogInformation("Removing livery for variant {VariantName} with ATC ID {AtcId}", variantName, atcId);
        
        var variantConfigPath = simulatorService.GetVariantConfigurationPath(variantName);

        AircraftConfiguration aircraftConfiguration;
        using (var aircraftConfig = new StreamReader(variantConfigPath))
        {
            aircraftConfiguration = await aircraftConfigurationDeserializer.DeserializeAsync(aircraftConfig);
            if (aircraftConfiguration is null)
                return;
        }

        var liverySection = aircraftConfiguration.FindFlightSimSection(atcId);
        if (liverySection is null)
            return;

        aircraftConfiguration.RemoveSection(liverySection);

        await using (var aircraftConfigStream = new StreamWriter(variantConfigPath))
        {
            await aircraftConfigurationSerializer.SerializeAsync(aircraftConfiguration, aircraftConfigStream);
        }
    }
}