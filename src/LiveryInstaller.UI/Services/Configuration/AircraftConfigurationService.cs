using System.IO;
using LiveryInstaller.UI.Models.INI;
using LiveryInstaller.UI.Services.Parsing;
using Microsoft.Extensions.Logging;

namespace LiveryInstaller.UI.Services.Configuration;

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

        var deserializedTempConfig = await LoadAircraftConfigurationAsync(tempConfigPath);
        var deserializedAircraftConfig = await LoadAircraftConfigurationAsync(aircraftConfigPath);

        deserializedAircraftConfig.Merge(deserializedTempConfig);

        await SaveAircraftConfigurationAsync(deserializedAircraftConfig, aircraftConfigPath);
    }

    /// <inheritdoc />
    public async Task RemoveLiveryAsync(string variantName, string atcId)
    {
        logger.LogInformation("Removing livery for variant {VariantName} with ATC ID {AtcId}", variantName, atcId);

        var variantConfigPath = simulatorService.GetVariantConfigurationPath(variantName);

        var aircraftConfiguration = await LoadAircraftConfigurationAsync(variantConfigPath);

        var liverySection = aircraftConfiguration.FindFlightSimSection(atcId);
        if (liverySection is null)
            return;

        aircraftConfiguration.RemoveSection(liverySection);
        
        await SaveAircraftConfigurationAsync(aircraftConfiguration, variantConfigPath);
    }

    public async Task<AircraftConfiguration> LoadAircraftConfigurationAsync(string configurationFile)
    {
        using var aircraftConfig = new StreamReader(configurationFile);
        
        return await aircraftConfigurationDeserializer.DeserializeAsync(aircraftConfig);
    }

    private async Task SaveAircraftConfigurationAsync(AircraftConfiguration configuration, string configurationFile)
    {
        await using var aircraftConfigStream = new StreamWriter(configurationFile);
        await aircraftConfigurationSerializer.SerializeAsync(configuration, aircraftConfigStream);
    }
}