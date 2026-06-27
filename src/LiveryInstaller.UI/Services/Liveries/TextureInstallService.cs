using System.IO;
using LiveryInstaller.UI.Models.Configuration;
using Microsoft.Extensions.Logging;

namespace LiveryInstaller.UI.Services.Liveries;

/// <inheritdoc />
public sealed class TextureInstallService(
    IFileSystem fileSystem,
    ISimulatorService simulatorService,
    ILogger<TextureInstallService> logger) : ITextureInstallService
{
    /// <inheritdoc />
    public void InstallTexture(SimulatorType simulatorType, string sourceArchiveDirectory, string variantName, string textureId)
    {
        logger.LogInformation("Installing texture {TextureId} for variant {VariantName}", textureId, variantName);
        
        fileSystem.DirectoryCopy(
            Path.Combine(sourceArchiveDirectory, $"Texture.{textureId}"),
            simulatorService.GetLiveryPath(simulatorType, variantName, textureId));
    }

    /// <inheritdoc />
    public void UninstallTexture(SimulatorType simulatorType, string variantName, string textureId)
    {
        logger.LogInformation("Uninstalling texture {TextureId} for variant {VariantName}", textureId, variantName);
        
        var liveryPath = simulatorService.GetLiveryPath(simulatorType, variantName, textureId);

        if (fileSystem.DirectoryExists(liveryPath))
            fileSystem.DirectoryDelete(liveryPath);
    }
}