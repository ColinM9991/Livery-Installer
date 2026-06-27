using System.IO;
using LiveryInstaller.UI.Models.Configuration;

namespace LiveryInstaller.UI.Services.Liveries;

/// <inheritdoc />
public sealed class TextureInstallService(
    IFileSystem fileSystem,
    ISimulatorService simulatorService) : ITextureInstallService
{
    /// <inheritdoc />
    public void InstallTexture(SimulatorType simulatorType, string sourceArchiveDirectory, string variantName, string textureId)
    {
        fileSystem.DirectoryCopy(
            Path.Combine(sourceArchiveDirectory, $"Texture.{textureId}"),
            simulatorService.GetLiveryPath(simulatorType, variantName, textureId));
    }

    /// <inheritdoc />
    public void UninstallTexture(SimulatorType simulatorType, string variantName, string textureId)
    {
        var liveryPath = simulatorService.GetLiveryPath(simulatorType, variantName, textureId);

        if (fileSystem.DirectoryExists(liveryPath))
            fileSystem.DirectoryDelete(liveryPath);
    }
}