namespace LiveryInstaller.UI.Services.Configuration;

/// <summary>
/// Represents a service that can user settings.
/// </summary>
public interface IWriteableConfigurationStore<T>
{
    /// <summary>
    /// Saves the settings of <typeparamref name="T"/>..
    /// </summary>
    /// <param name="settings">The settings to save.</param>
    /// <returns>A Task that represents the asynchronous operation.</returns>
    Task WriteAsync(T settings);
}