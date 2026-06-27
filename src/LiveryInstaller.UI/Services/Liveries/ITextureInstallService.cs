using LiveryInstaller.UI.Models.Configuration;

namespace LiveryInstaller.UI.Services.Liveries;

/// <summary>
/// Represents a service that can install and uninstall textures.
/// </summary>
[LoggingDecorator]
public interface ITextureInstallService
{
    /// <summary>
    /// Installs a requested texture.
    /// </summary>
    /// <param name="simulatorType">The simulator the texture is being installed for.</param>
    /// <param name="sourceArchiveDirectory">The path to the source archive directory.</param>
    /// <param name="variantName">The name of the aircraft variant.</param>
    /// <param name="textureId">The texture ID of the livery.</param>
    void InstallTexture(SimulatorType simulatorType, string sourceArchiveDirectory, string variantName, string textureId);

    /// <summary>
    /// Uninstalls a requested texture.
    /// </summary>
    /// <param name="simulatorType">The simulator the texture is being uninstalled from.</param>
    /// <param name="variantName">The name of the aircraft variant.</param>
    /// <param name="textureId">The texture ID of the livery.</param>
    void UninstallTexture(SimulatorType simulatorType, string variantName, string textureId);
}