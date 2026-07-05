using System.IO;
using System.Text.Json;
using LiveryInstaller.Library.Models.Configuration;
using LiveryInstaller.Library.Services;
using LiveryInstaller.Library.Services.Configuration;
using LiveryInstaller.UI.Helpers;

namespace LiveryInstaller.UI.Services.Configuration;

/// <inheritdoc />
public sealed class UserConfigurationStore(IFileSystem fileSystem) : IWriteableConfigurationStore<UserSettings>
{
    public static string SettingsFile => Path.Combine(Paths.SettingsDirectory, "appsettings.json");
    
    /// <inheritdoc />
    public async Task WriteAsync(UserSettings settings)
    {
        ArgumentNullException.ThrowIfNull(settings);

        var fileOpenMode = fileSystem.FileExists(SettingsFile) ? FileMode.Truncate : FileMode.Create;
        await using var file = new FileStream(SettingsFile, fileOpenMode, FileAccess.Write);
        
        await JsonSerializer.SerializeAsync(file, new UserConfiguration(settings));
    }
}