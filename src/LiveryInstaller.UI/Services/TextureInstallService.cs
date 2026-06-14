using System.IO;
using Microsoft.Extensions.Logging;

namespace LiveryInstaller.UI.Services;

/// <inheritdoc />
public sealed class TextureInstallService(
    IFileSystem fileSystem,
    ISimulatorService simulatorService,
    ILogger<TextureInstallService> logger) : ITextureInstallService
{
    /// <inheritdoc />
    public void InstallTexture(string sourceArchiveDirectory, string variantName, string textureId)
    {
        logger.LogInformation("Installing texture {TextureId} for variant {VariantName}", textureId, variantName);
        
        fileSystem.DirectoryCopy(
            Path.Combine(sourceArchiveDirectory, $"Texture.{textureId}"),
            simulatorService.GetLiveryPath(variantName, textureId));
    }

    /// <inheritdoc />
    public void UninstallTexture(string variantName, string textureId)
    {
        logger.LogInformation("Uninstalling texture {TextureId} for variant {VariantName}", textureId, variantName);
        
        var liveryPath = simulatorService.GetLiveryPath(variantName, textureId);

        if (fileSystem.DirectoryExists(liveryPath))
            fileSystem.DirectoryDelete(liveryPath);
    }
}