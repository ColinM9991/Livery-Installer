namespace LiveryInstaller.UI.Services.Liveries;

/// <summary>
/// Represents a service that can install and uninstall textures.
/// </summary>
public interface ITextureInstallService
{
    /// <summary>
    /// Installs a requested texture.
    /// </summary>
    /// <param name="sourceArchiveDirectory">The path to the source archive directory.</param>
    /// <param name="variantName">The name of the aircraft variant.</param>
    /// <param name="textureId">The texture ID of the livery.</param>
    void InstallTexture(string sourceArchiveDirectory, string variantName, string textureId);

    /// <summary>
    /// Uninstalls a requested texture.
    /// </summary>
    /// <param name="variantName">The name of the aircraft variant.</param>
    /// <param name="textureId">The texture ID of the livery.</param>
    void UninstallTexture(string variantName, string textureId);
}