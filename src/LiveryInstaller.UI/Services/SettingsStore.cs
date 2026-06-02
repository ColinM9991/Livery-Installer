using System.IO;
using System.Text.Json;
using LiveryInstaller.UI.Models.Configuration;

namespace LiveryInstaller.UI.Services;

/// <inheritdoc />
public sealed class SettingsStore : ISettingsStore
{
    private static string SettingsDirectory =>
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "LiveryInstaller");
    
    public static string SettingsFile => Path.Combine(SettingsDirectory, "appsettings.json");
    
    /// <inheritdoc />
    public async Task SaveSettingsAsync(UserSettings settings)
    {
        ArgumentNullException.ThrowIfNull(settings);
        
        if (!Directory.Exists(SettingsDirectory))
        {
            Directory.CreateDirectory(SettingsDirectory);
        }

        var fileOpenMode = File.Exists(SettingsFile) ? FileMode.Truncate : FileMode.Create;
        await using var file = new FileStream(SettingsFile, fileOpenMode, FileAccess.Write);
        
        await JsonSerializer.SerializeAsync(file, new UserConfiguration(settings));
    }
}