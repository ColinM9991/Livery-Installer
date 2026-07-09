using LiveryInstaller.Library.Models.Configuration;
using LiveryInstaller.Library.Models.INI;
using LiveryInstaller.Library.Services.Parsing;
using AircraftConfiguration = LiveryInstaller.Library.Models.INI.AircraftConfiguration;

namespace LiveryInstaller.Library.Services.Configuration;

/// <inheritdoc />
public sealed class AircraftConfigurationService(
    ISimulatorService simulatorService,
    IAircraftConfigurationDeserializer aircraftConfigurationDeserializer,
    IAircraftConfigurationSerializer aircraftConfigurationSerializer) : IAircraftConfigurationService
{
    /// <inheritdoc />
    public async Task AddLiveryAsync(SimulatorType simulatorType, string variantName, string archiveDirectory)
    {
        var aircraftConfigPath = simulatorService.GetVariantConfigurationPath(simulatorType, variantName);
        var tempConfigPath = Path.Combine(archiveDirectory, "Config.cfg");

        var deserializedTempConfig = await LoadAircraftConfigurationAsync(tempConfigPath);
        var deserializedAircraftConfig = await LoadAircraftConfigurationAsync(aircraftConfigPath);

        deserializedAircraftConfig.Merge(deserializedTempConfig);

        await SaveAircraftConfigurationAsync(deserializedAircraftConfig, aircraftConfigPath);
    }

    /// <inheritdoc />
    public async Task RemoveLiveryAsync(SimulatorType simulatorType, string variantName, string atcId)
    {
        var variantConfigPath = simulatorService.GetVariantConfigurationPath(simulatorType, variantName);

        var aircraftConfiguration = await LoadAircraftConfigurationAsync(variantConfigPath);

        var liverySection = aircraftConfiguration.FindFlightSimSection(atcId);
        if (liverySection is null)
            return;

        aircraftConfiguration.RemoveSection(liverySection);
        
        await SaveAircraftConfigurationAsync(aircraftConfiguration, variantConfigPath);
    }

    public async Task<AircraftConfiguration> LoadAircraftConfigurationAsync(string configurationFile) => await LoadAircraftFileAsync<AircraftConfiguration>(configurationFile);

    public async Task<AircraftSettings> LoadAircraftSettingsAsync(string configurationFile) => await LoadAircraftFileAsync<AircraftSettings>(configurationFile);

    private async Task<T> LoadAircraftFileAsync<T>(string configurationFile)
    {
        using var aircraftConfig = new StreamReader(configurationFile);
        
        return await aircraftConfigurationDeserializer.DeserializeAsync<T>(aircraftConfig);
    }

    private async Task SaveAircraftConfigurationAsync(AircraftConfiguration configuration, string configurationFile)
    {
        await using var aircraftConfigStream = new StreamWriter(configurationFile);
        await aircraftConfigurationSerializer.SerializeAsync(configuration, aircraftConfigStream);
    }
}