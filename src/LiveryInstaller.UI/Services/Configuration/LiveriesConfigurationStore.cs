using System.IO;
using System.Text.Json;
using LiveryInstaller.UI.Helpers;
using LiveryInstaller.UI.Models.Configuration;

namespace LiveryInstaller.UI.Services.Configuration;

public class LiveriesConfigurationStore(IFileSystem fileSystem) :
    IReadableConfigurationStore<LiveriesConfiguration>,
    IWriteableConfigurationStore<LiveriesConfiguration>
{
    public static readonly string ConfigurationFile = Path.Combine(Paths.SettingsDirectory, "userLiveryConfiguration.json");

    public async Task<LiveriesConfiguration> ReadAsync()
    {
        if (!fileSystem.FileExists(ConfigurationFile)) return new LiveriesConfiguration();
        
        await using var file = new FileStream(ConfigurationFile, FileMode.OpenOrCreate);

        return await JsonSerializer.DeserializeAsync<LiveriesConfiguration>(file);
    }

    public Task WriteAsync(LiveriesConfiguration settings)
    {
        ArgumentNullException.ThrowIfNull(settings);

        var fileMode = fileSystem.FileExists(ConfigurationFile) ? FileMode.Truncate : FileMode.Create;

        using var file = new FileStream(ConfigurationFile, fileMode);

        return JsonSerializer.SerializeAsync(file, settings);
    }
}