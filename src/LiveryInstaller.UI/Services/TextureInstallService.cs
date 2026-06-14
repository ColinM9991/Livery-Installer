using System.IO;
using Microsoft.Extensions.Logging;

namespace LiveryInstaller.UI.Services;

/// <inheritdoc />
public sealed class TextureInstallService(
    ISimulatorService simulatorService,
    ILogger<TextureInstallService> logger) : ITextureInstallService
{
    /// <inheritdoc />
    public void InstallTexture(string sourceArchiveDirectory, string variantName, string textureId)
    {
        logger.LogInformation("Installing texture {TextureId} for variant {VariantName}", textureId, variantName);
        
        CopyDirectory(
            Path.Combine(sourceArchiveDirectory, $"Texture.{textureId}"),
            simulatorService.GetLiveryPath(variantName, textureId));
    }

    /// <inheritdoc />
    public void UninstallTexture(string variantName, string textureId)
    {
        logger.LogInformation("Uninstalling texture {TextureId} for variant {VariantName}", textureId, variantName);
        
        var liveryPath = simulatorService.GetLiveryPath(variantName, textureId);

        if (Directory.Exists(liveryPath))
            Directory.Delete(liveryPath, true);
    }

    private void CopyDirectory(string source, string destination)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(destination);

        logger.LogInformation("Copying directory {Source} to {Destination}", source, destination);

        if (Directory.Exists(destination))
        {
            logger.LogWarning("Directory {Destination} already exists, deleting it", destination);
            Directory.Delete(destination, true);
        }

        Directory.CreateDirectory(destination);

        foreach (var file in Directory.GetFiles(source))
        {
            var targetFile = Path.Combine(destination, Path.GetFileName(file));
            File.Copy(file, targetFile, overwrite: true);
        }
    }
}