using LiveryInstaller.UI.Models.Configuration;

namespace LiveryInstaller.UI.Services;

/// <summary>
/// Represents a service that can save user settings.
/// </summary>
public interface ISettingsStore
{
    /// <summary>
    /// Save user settings.
    /// </summary>
    /// <param name="settings">The settings to save.</param>
    /// <returns>A Task that represents the asynchronous operation.</returns>
    Task SaveSettingsAsync(UserSettings settings);
}